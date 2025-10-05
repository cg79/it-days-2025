using System.Data;
using System.Text.Json;
using Dapper;

namespace ef_dapper_CustomDataSeed;

public class CustomDataSeed
{
    public async Task SeedDatabaseAsync(IDbConnection connection, string json)
    {
        var configs = JsonSerializer.Deserialize<List<SeedConfig>>(json);
        foreach (var config in configs)
        {
            var fkCache = new Dictionary<string, List<object>>();

            // Pre-fetch foreign key values if needed
            foreach (var col in config.ColumnValues)
            {
                var val = col.Value.Value;
                if (val.Type == "table" && val.Column != null)
                {
                    var parts = val.Column.Split('.');
                    string fkTable = parts[0];
                    string fkColumn = parts[1];

                    var results = await connection.QueryAsync<object>($"SELECT {fkColumn} FROM {fkTable}");
                    fkCache[val.Column] = results.ToList();
                }
            }

            // Generate and insert records
            for (int i = 0; i < config.Records; i++)
            {
                var insertColumns = new List<string>();
                var insertParams = new List<string>();
                var parameters = new DynamicParameters();

                foreach (var col in config.ColumnValues)
                {
                    string columnName = col.Key;
                    var colConfig = col.Value.Value;
                    object value = null;

                    switch (colConfig.Type)
                    {
                        case "random":
                            value = RandomValueGenerator.GenerateRandomValue(typeof(decimal), colConfig.Min,
                                colConfig.Max);
                            break;

                        case "table":
                            var fkValues = fkCache[colConfig.Column];
                            value = fkValues[new Random().Next(fkValues.Count)];
                            break;

                        case "fixed":
                            value = colConfig.FixedValue;
                            break;

                        default:
                            // No config — generate default value
                            value = $"Auto_{Guid.NewGuid().ToString().Substring(0, 6)}";
                            break;
                    }

                    insertColumns.Add(columnName);
                    insertParams.Add($"@{columnName}");
                    parameters.Add($"@{columnName}", value);
                }

                var sql = $"INSERT INTO {config.TableName} ({string.Join(",", insertColumns)}) " +
                          $"VALUES ({string.Join(",", insertParams)})";
                await connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}