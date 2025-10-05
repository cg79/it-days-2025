namespace ef_dapper_CustomDataSeed;

public class SeedConfig
{
    public string TableName { get; set; }
    public int Records { get; set; }
    public Dictionary<string, ColumnConfig> ColumnValues { get; set; }
}

public class ColumnConfig
{
    public ColumnValue Value { get; set; }
}

public class ColumnValue
{
    public string Type { get; set; }           // "random", "table", "fixed"
    public string Column { get; set; }        // for table lookup
    public object FixedValue { get; set; }    // for fixed value
    public double? Min { get; set; }          // for random
    public double? Max { get; set; }          // for random
}