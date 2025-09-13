namespace ef_base_repository;

using System.Text.Json;
using System.Text.Json.Nodes;

public static class FilterParser
{
    private static readonly Dictionary<FilterOperator, string> OperatorMap = new()
    {
        [FilterOperator.Eq] = "==",
        [FilterOperator.Neq] = "!=",
        [FilterOperator.Lt] = "<",
        [FilterOperator.Lte] = "<=",
        [FilterOperator.Gt] = ">",
        [FilterOperator.Gte] = ">=",
        [FilterOperator.Contains] = "Contains",
        [FilterOperator.StartsWith] = "StartsWith",
        [FilterOperator.EndsWith] = "EndsWith"
    };

    public static string Parse(FilterGroup group)
    {
        var expressions = new List<string>();

        foreach (var filter in group.Filters)
        {
            switch (filter)
            {
                case FilterCondition cond:
                    expressions.Add(ParseCondition(cond));
                    break;

                case FilterGroup nestedGroup:
                    expressions.Add($"({Parse(nestedGroup)})");
                    break;

                default:
                    throw new InvalidOperationException("Unsupported filter type");
            }
        }

        var logic = group.Logic?.ToUpper() ?? "AND";
        return string.Join($" {logic} ", expressions);
    }

    private static string ParseCondition(FilterCondition cond)
    {
        var field = cond.Field;
        var op = cond.Operator;
        var value = cond.Value;

        if (!OperatorMap.ContainsKey(op))
            throw new InvalidOperationException($"Unsupported operator: {op}");

        if (op is FilterOperator.Contains or FilterOperator.StartsWith or FilterOperator.EndsWith)
        {
            return $"{field}.{OperatorMap[op]}(\"{value}\")";
        }

        return value is int or long or double or float or decimal
            ? $"{field} {OperatorMap[op]} {value}"
            : $"{field} {OperatorMap[op]} \"{value}\"";
    }
}


public enum FilterOperator
{
    Eq,
    Neq,
    Lt,
    Lte,
    Gt,
    Gte,
    Contains,
    StartsWith,
    EndsWith,
    IsNull,
    IsNotNull
}

public class FilterGroup
{
    public string Logic { get; set; } = "and"; 
    public List<object> Filters { get; set; } = new(); 
}

public class FilterCondition
{
    public string Field { get; set; }
    public FilterOperator Operator { get; set; }  // <-- now enum
    public object Value { get; set; }
}