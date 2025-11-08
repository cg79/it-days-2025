
using Dapper;

namespace ef_base_repository;

public static class DapperFilterParser
{
    private static readonly Dictionary<FilterOperator, string> SqlOperators = new()
    {
        [FilterOperator.Eq] = "=",
        [FilterOperator.Neq] = "<>",
        [FilterOperator.Lt] = "<",
        [FilterOperator.Lte] = "<=",
        [FilterOperator.Gt] = ">",
        [FilterOperator.Gte] = ">=",
        [FilterOperator.Contains] = "LIKE",
        [FilterOperator.StartsWith] = "LIKE",
        [FilterOperator.EndsWith] = "LIKE"
    };

    public static (string whereClause, DynamicParameters parameters) Parse(FilterGroup group)
    {
        var parameters = new DynamicParameters();
        var whereClause = ParseGroup(group, parameters);
        return (whereClause, parameters);
    }

    private static string ParseGroup(FilterGroup group, DynamicParameters parameters)
    {
        var expressions = new List<string>();

        foreach (var filter in group.Filters)
        {
            switch (filter)
            {
                case FilterCondition cond:
                    expressions.Add(ParseCondition(cond, parameters));
                    break;

                case FilterGroup nestedGroup:
                    expressions.Add($"({ParseGroup(nestedGroup, parameters)})");
                    break;
            }
        }

        var logic = group.Logic?.ToUpper() ?? "AND";
        return string.Join($" {logic} ", expressions);
    }

    private static string ParseCondition(FilterCondition cond, DynamicParameters parameters)
    {
        var field = cond.Field;
        var op = cond.Operator;
        var paramName = $"@p{parameters.ParameterNames.Count()}";

        object value = cond.Value;

        switch (op)
        {
            case FilterOperator.Contains:
                parameters.Add(paramName, $"%{value}%");
                return $"{field} {SqlOperators[op]} {paramName}";

            case FilterOperator.StartsWith:
                parameters.Add(paramName, $"{value}%");
                return $"{field} {SqlOperators[op]} {paramName}";

            case FilterOperator.EndsWith:
                parameters.Add(paramName, $"%{value}");
                return $"{field} {SqlOperators[op]} {paramName}";

            default:
                parameters.Add(paramName, value);
                return $"{field} {SqlOperators[op]} {paramName}";
        }
    }
}
