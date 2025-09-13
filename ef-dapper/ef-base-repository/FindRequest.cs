namespace ef_base_repository;

public enum SqlOperator
{
    Equals,
    NotEquals,
    GreaterThan,
    GreaterOrEqual,
    LessThan,
    LessOrEqual,
    StartsWith,
    EndsWith,
    Contains,
    In
}


public class QueryCondition
{
    public string Field { get; set; } = string.Empty;
    public SqlOperator Operator { get; set; }
    public object? Value { get; set; }
}

public class QueryFilter
{
    public List<QueryCondition> Conditions { get; set; } = new();
}



