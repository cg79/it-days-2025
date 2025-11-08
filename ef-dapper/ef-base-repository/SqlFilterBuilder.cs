namespace ef_base_repository;

public static class SqlFilterBuilder
{
    public static string BuildWhereClause(FilterGroup group)
    {
        var conditions = new List<string>();

        foreach (var filter in group.Filters)
        {
            if (filter is FilterCondition condition)
            {
                conditions.Add(BuildCondition(condition));
            }
            else if (filter is FilterGroup subGroup)
            {
                var subClause = BuildWhereClause(subGroup);
                if (!string.IsNullOrWhiteSpace(subClause))
                {
                    conditions.Add($"({subClause})");
                }
            }
        }

        var logic = group.Logic.Equals("or", StringComparison.OrdinalIgnoreCase) ? " OR " : " AND ";
        return string.Join(logic, conditions);
    }

    private static string BuildCondition(FilterCondition condition)
    {
        string value = condition.Value.ToString();
        string sqlOperator;
        string formattedValue;

        switch (condition.Operator)
        {
            case FilterOperator.Eq:
                sqlOperator = "=";
                formattedValue = $"'{value}'";
                break;
            case FilterOperator.Neq:
                sqlOperator = "<>";
                formattedValue = $"'{value}'";
                break;
            case FilterOperator.Gt:
                sqlOperator = ">";
                formattedValue = $"{value}";
                break;
            case FilterOperator.Gte:
                sqlOperator = ">=";
                formattedValue = $"{value}";
                break;
            case FilterOperator.Lt:
                sqlOperator = "<";
                formattedValue = $"{value}";
                break;
            case FilterOperator.Lte:
                sqlOperator = "<=";
                formattedValue = $"{value}";
                break;
            case FilterOperator.Contains:
                sqlOperator = "LIKE";
                formattedValue = $"'%{value}%'";
                break;
            case FilterOperator.StartsWith:
                sqlOperator = "LIKE";
                formattedValue = $"'{value}%'";
                break;
            case FilterOperator.EndsWith:
                sqlOperator = "LIKE";
                formattedValue = $"'%{value}'";
                break;
            default:
                throw new NotSupportedException($"Operator {condition.Operator} not supported");
        }

        return $"{condition.Field} {sqlOperator} {formattedValue}";
    }
}
