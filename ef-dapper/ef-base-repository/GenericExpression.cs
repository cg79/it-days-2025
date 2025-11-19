using System.Dynamic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Dapper;

namespace ef_base_repository;
public abstract class GenericExpression
{
    public static Expression<Func<T, bool>> CreateFilterExpression<T>(string filter)
    {
        return DynamicExpressionParser.ParseLambda<T, bool>(new ParsingConfig(), false, filter);
    }

    public static IQueryable<T> ApplySorting<T>(IQueryable<T> source, Dictionary<string, bool>? sortCriteria)
    {
        if (sortCriteria == null || !sortCriteria.Any())
        {
            return source;
        }

        var orderByString = string.Join(", ",
            sortCriteria.Select(kvp => $"{kvp.Key} {(kvp.Value ? "desc" : "asc")}"));

        return source.OrderBy(orderByString);
    }
    
    public static string CreateUpdateQuery(object entity, string tableName, string keyName = "Id")
    {
        if (entity is IDictionary<string, object> dict) // ExpandoObject or Dictionary
        {
            if (!dict.ContainsKey(keyName))
                throw new ArgumentException($"Key '{keyName}' not found in entity");

            var setClauses = dict
                .Where(kvp => kvp.Key != keyName)
                .Select(kvp => $"{kvp.Key} = @{kvp.Key}");

            var setClause = string.Join(", ", setClauses);

            return $"UPDATE {tableName} SET {setClause} WHERE {keyName} = @{keyName};";
        }
        else // POCO with reflection
        {
            var type = entity.GetType();
            var props = type.GetProperties()
                .Where(p => p.CanRead && p.Name != keyName) // skip PK
                .ToList();

            if (type.GetProperty(keyName) == null)
                throw new ArgumentException($"Key property '{keyName}' not found on type '{type.Name}'");

            var setClauses = props.Select(p => $"{p.Name} = @{p.Name}");
            var setClause = string.Join(", ", setClauses);

            return $"UPDATE {tableName} SET {setClause} WHERE {keyName} = @{keyName};";
        }
    }

    public static FormattableString CreateFormatableUpdateQuery_NOTUSED(object entity, string tableName, string keyName = "Id")
    {
        var type = entity.GetType();
        var props = type.GetProperties()
            .Where(p => p.CanRead && p.Name != keyName) // skip PK
            .ToList();

        var keyProperty = type.GetProperty(keyName);
        if (keyProperty == null)
            throw new ArgumentException($"Key property '{keyName}' not found on type '{type.Name}'");

        // Build SET clause with interpolation
        var assignments = props
            .Select(p => $"{p.Name} = {{{p.Name}}}")
            .ToList();

        var setClause = string.Join(", ", assignments);

        // Build FormattableString dynamically
        var values = props.Select(p => p.GetValue(entity)).ToList();
        values.Add(keyProperty.GetValue(entity)!); // key last

        // Create raw SQL with placeholders {0}, {1}, etc.
        var sql = $"UPDATE {tableName} SET {setClause} WHERE {keyName} = {{{keyName}}};";

        // ⚠ We cannot directly interpolate property names, so we map them
        // into an object array that EF will treat as parameters
        return FormattableStringFactory.Create(sql, values.ToArray());
    }

    public static FormattableString CreateFormattableUpdateQuery(object entity, string tableName, string keyName = "Id")
    {
        var type = entity.GetType();
        var props = type.GetProperties()
            .Where(p => p.CanRead && p.Name != keyName) // skip PK
            .ToList();

        var keyProperty = type.GetProperty(keyName);
        if (keyProperty == null)
            throw new ArgumentException($"Key property '{keyName}' not found on type '{type.Name}'");

        var assignments = props
            .Select((p, i) => $"{p.Name} = {{{i}}}")
            .ToList();

        var setClause = string.Join(", ", assignments);

        // WHERE clause will be the last placeholder
        var sql = $"UPDATE {tableName} SET {setClause} WHERE {keyName} = {{{props.Count}}};";

        // Collect values in order
        var values = props.Select(p => p.GetValue(entity)).ToList();
        values.Add(keyProperty.GetValue(entity)!); // add key at the end

        return FormattableStringFactory.Create(sql, values.ToArray());
    }

    public static (string Sql, object Parameters) CreateUpdateQueryAndParams(object entity, string tableName, string keyName = "Id")
    {
        var type = entity.GetType();
        var props = type.GetProperties()
            .Where(p => p.CanRead && p.Name != keyName) // skip PK
            .ToList();

        var keyProperty = type.GetProperty(keyName);
        if (keyProperty == null)
            throw new ArgumentException($"Key property '{keyName}' not found on type '{type.Name}'");

        // build assignments
        var assignments = props
            .Select(p => $"{p.Name} = @{p.Name}")
            .ToList();

        var setClause = string.Join(", ", assignments);
        var sql = $"UPDATE {tableName} SET {setClause} WHERE {keyName} = @{keyName};";

        // build parameters object for Dapper
        var parameters = new DynamicParameters();
        foreach (var p in props)
        {
            parameters.Add("@" + p.Name, p.GetValue(entity));
        }
        parameters.Add("@" + keyName, keyProperty.GetValue(entity));

        return (sql, parameters);
    }

    public static (string Sql, object Parameters) CreateInsertQuery(object entity, string tableName, string keyName = "Id")
    {
        var type = entity.GetType();
        var props = type.GetProperties()
            .Where(p => p.CanRead && p.Name != keyName) // skip PK (assumed identity/auto-generated)
            .ToList();

        if (!props.Any())
            throw new ArgumentException($"No insertable properties found on type '{type.Name}'");

        // Build column and parameter lists
        var columns = string.Join(", ", props.Select(p => p.Name));
        var paramNames = string.Join(", ", props.Select(p => "@" + p.Name));

        var sql = $"INSERT INTO {tableName} ({columns}) VALUES ({paramNames});SELECT LAST_INSERT_ID();";

        var parameters = new DynamicParameters();
        foreach (var p in props)
        {
            parameters.Add("@" + p.Name, p.GetValue(entity));
        }
        return (sql, parameters);
    }

    public static (string Sql, object Parameters) CreateInsertQueryFromExpando(object entity, string tableName, string keyName = "Id")
    {
        IDictionary<string, object> propsDict;

        if (entity is ExpandoObject expando)
        {
            propsDict = (IDictionary<string, object>)expando;
        }
        else
        {
            propsDict = entity.GetType()
                .GetProperties()
                .Where(p => p.CanRead && p.Name != keyName)
                .ToDictionary(p => p.Name, p => p.GetValue(entity));
        }

        if (!propsDict.Any())
            throw new ArgumentException($"No insertable properties found on type '{entity.GetType().Name}'");

        var columns = string.Join(", ", propsDict.Keys);
        var paramNames = string.Join(", ", propsDict.Keys.Select(k => "@" + k));

        var sql = $"INSERT INTO {tableName} ({columns}) VALUES ({paramNames}); SELECT LAST_INSERT_ID();";

        var parameters = new DynamicParameters();
        foreach (var kvp in propsDict)
        {
            parameters.Add("@" + kvp.Key, kvp.Value);
        }

        return (sql, parameters);
    }

}
