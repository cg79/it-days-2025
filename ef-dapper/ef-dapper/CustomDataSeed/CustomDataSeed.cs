using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;
using Bogus;
using Dapper;
using ef_base_repository;

namespace ef_dapper_CustomDataSeed;

public class CustomDataSeed
{
    public async Task RunAsync(DataContext db, string jsonConfigPath)
    {
        var jsSeed = File.ReadAllText(jsonConfigPath);
        var configs = JsonSerializer.Deserialize<List<SeedConfig>>(jsSeed);
        if (configs == null || configs.Count == 0)
        {
            return;
        }

        using var conn = db.GetDbConnection();
        if (conn.State == ConnectionState.Closed)
        {
            conn.Open();
        }

        foreach (var seedConfig in configs)
        {
            await SeedTableAsync(conn, seedConfig);
        } 
            
    }

    public async Task<SeedConfig> CreateSeedFeeldsForATable(DataContext db, string tableName)
    {
        using var conn = db.GetDbConnection();
        if (conn.State == ConnectionState.Closed)
        {
            conn.Open();
        }

        var seedConfig = new SeedConfig()
        {
            table = tableName,
        };
        await PrepareTableAsync(conn, seedConfig);
        return seedConfig;
    }

    public async Task<string> GetTableFieldsAsJson(DataContext db, string tableName)
    {
        var seedConfig = await CreateSeedFeeldsForATable(db, tableName);
        return  JsonSerializer.Serialize( seedConfig );
    }
    private async Task SeedTableAsync(DbConnection conn, SeedConfig config)
    {
        var lookupCache = new Dictionary<string, List<object>>();
        var faker = new Faker();
        Dictionary<string, object> properties = new();
        
        await PrepareTableAsync(conn, config);
        

        // Pre-load lookup fields into memory
        foreach (var f in config.fields.Where(x => x.Value.type == "lookup"))
        {
            string key = $"{f.Value.table}.{f.Value.field}";
            lookupCache[key] = await LoadLookupAsync(conn, f.Value.table, f.Value.field);
        }

        for (int i = 0; i < config.count; i++)
        {
            foreach (var (col, rule) in config.fields)
            {
                object value =
                    rule.type == null
                        ? CallBogusMethod(faker, rule.method)
                        : rule.type switch
                        {
                            "" => CallBogusMethod(faker, rule.method),
                            "faker" => CallBogusMethod(faker, rule.method),
                            "range" => new Random().Next(rule.min, rule.max + 1),
                            "lookup" => PickRandom(lookupCache[$"{rule.table}.{rule.field}"]),
                            _ => CallBogusMethod(faker, rule.method)
                        };

                properties[col] = value;
            }

            var obj = CreateDynamicObject(properties);
            await InsertObject(conn, obj, config.table);
        }
    }

    private async Task PrepareTableAsync(DbConnection conn, SeedConfig config)
    {
        var dbColumns = await GetTableColumnsAsync(conn, config.table);

        if (config.fields == null)
        {
            config.fields = new Dictionary<string, FieldRule>();
        }

        dbColumns.ForEach(c =>
        {
            if (string.IsNullOrEmpty(c.COLUMN_KEY))
            {
                if (!config.fields.TryGetValue(c.COLUMN_NAME, out var jsonRule))
                {
                    Console.WriteLine($"{c.COLUMN_NAME} : <null>");
                    jsonRule = CreateFieldRule(c);
                    config.fields[c.COLUMN_NAME] = jsonRule;
                }
            }
        });
    }

    private FieldRule CreateFieldRule(ColumnInfo columnInfo)
    {
        var rule = new FieldRule();
        rule.field = columnInfo.COLUMN_NAME;
        rule.type = columnInfo.DATA_TYPE;
        rule.method = columnInfo.DATA_TYPE;
        
        switch (columnInfo.DATA_TYPE)
        {
            case "varchar":
            {
                rule.method = "name";
                break;
            }
            case "int":
            case "integer":
            {
                rule.method = "int:1:2000";
                break;
            }
            case "tinyint":
            {
                rule.method = "int:1:64";
                break;
            }
        }
        return rule;
    }

    private async Task<List<ColumnInfo>> GetTableColumnsAsync(DbConnection conn, string tableName)
    {
        var sql = @"
        SELECT 
            COLUMN_NAME,
            DATA_TYPE,
            CHARACTER_MAXIMUM_LENGTH,
            IS_NULLABLE,
            COLUMN_DEFAULT,
            COLUMN_KEY
        FROM information_schema.COLUMNS
        WHERE 
            -- TABLE_SCHEMA = @DbName AND 
          TABLE_NAME = @TableName;
    ";

        var columns = await conn.QueryAsync<ColumnInfo>(sql, new { TableName = tableName });
        return columns.AsList();
    }
    public async Task<object> InsertObject(DbConnection db, object entity, string table)
    {
        var (sql, parameters)  = GenericExpression.CreateInsertQueryFromExpando(entity, table);
        return await db.ExecuteScalarAsync<long>(sql, parameters);
    }
    
    public  dynamic CreateDynamicObject(Dictionary<string, object> properties)
    {
        dynamic obj = new ExpandoObject();
        var dict = (IDictionary<string, object>)obj;

        foreach (var kvp in properties)
        {
            dict[kvp.Key] = kvp.Value;
        }

        return obj;
    }
    
   
    public static object CreateRuntimeObject(Dictionary<string, Type> properties)
    {
        var asmName = new AssemblyName("DynamicAssembly");
        var asmBuilder = AssemblyBuilder.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
    
        var moduleBuilder = asmBuilder.DefineDynamicModule("MainModule");
    
        var typeBuilder = moduleBuilder.DefineType(
            "DynamicType",
            TypeAttributes.Public | TypeAttributes.Class
        );

        foreach (var prop in properties)
        {
            var fieldBuilder = typeBuilder.DefineField("_" + prop.Key, prop.Value, FieldAttributes.Private);

            var propertyBuilder = typeBuilder.DefineProperty(
                prop.Key,
                PropertyAttributes.HasDefault,
                prop.Value,
                null
            );

            // Getter
            var getter = typeBuilder.DefineMethod(
                "get_" + prop.Key,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                prop.Value,
                Type.EmptyTypes
            );

            var getterIL = getter.GetILGenerator();
            getterIL.Emit(OpCodes.Ldarg_0);
            getterIL.Emit(OpCodes.Ldfld, fieldBuilder);
            getterIL.Emit(OpCodes.Ret);

            // Setter
            var setter = typeBuilder.DefineMethod(
                "set_" + prop.Key,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                null,
                new Type[] { prop.Value }
            );

            var setterIL = setter.GetILGenerator();
            setterIL.Emit(OpCodes.Ldarg_0);
            setterIL.Emit(OpCodes.Ldarg_1);
            setterIL.Emit(OpCodes.Stfld, fieldBuilder);
            setterIL.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getter);
            propertyBuilder.SetSetMethod(setter);
        }

        var dynamicType = typeBuilder.CreateType();
        return Activator.CreateInstance(dynamicType);
    }

    private async Task<List<object>> LoadLookupAsync(DbConnection conn, string table, string field, 
        int maxRecords = 50)
    {
        var list = new List<object>();
        // string sql = $"SELECT TOP {maxRecords} {field} FROM {table} ORDER BY NEWID();";
        string sql = $"select {field} from {table} ORDER BY RAND() LIMIT {maxRecords}";
        using var reader = await conn.ExecuteReaderAsync(sql);
        while (await reader.ReadAsync())
            list.Add(reader.GetValue(0));
        return list;
    }

    private static T PickRandom<T>(List<T> list)
    {
        return list[new Random().Next(list.Count)];
    }

    private static object CallBogusMethod(Faker faker, string method)
    {
        if (faker == null)
            throw new ArgumentNullException(nameof(faker));

        // Normalize input (case-insensitive)
        method = method.Trim();

        // Split parameters: "method:param1:param2"
        string[] parts = method.Split(':');
        string name = parts[0].ToLowerInvariant();

        // Switch on method name
        switch (name)
        {
            case "name":
            case "username":
                return faker.Internet.UserName();

            case "fullname":
                return faker.Name.FullName();

            case "date":
                return faker.Date.Between(DateTime.MinValue, DateTime.MaxValue);
            
            case "datebetween":
                if (parts.Length != 3)
                    throw new ArgumentException("dateBetween requires 2 params: start, end");

                DateTime start = DateTime.Parse(parts[1]);
                DateTime end = DateTime.Parse(parts[2]);
                return faker.Date.Between(start, end);

            case "long":
                if (parts.Length != 3)
                    throw new ArgumentException("long requires 2 params: min, max");

                long min = long.Parse(parts[1]);
                long max = long.Parse(parts[2]);
                return faker.Random.Long(min, max);

            case "int":
            case "integer":
                if (parts.Length != 3)
                    throw new ArgumentException("int requires 2 params: min, max");

                int imin = int.Parse(parts[1]);
                int imax = int.Parse(parts[2]);
                return faker.Random.Int(imin, imax);

            case "string":
                // string:len
                int len = parts.Length > 1 ? int.Parse(parts[1]) : 10;
                return faker.Random.AlphaNumeric(len);

            case "guid":
                return Guid.NewGuid();

            default:
                throw new NotSupportedException($"Unknown bogus method: {name}");
        }
    }

}
