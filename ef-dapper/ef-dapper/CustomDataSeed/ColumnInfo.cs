namespace ef_dapper_CustomDataSeed;

public class ColumnInfo
{
    public string COLUMN_NAME { get; set; }
    public string DATA_TYPE { get; set; }
    public int? CHARACTER_MAXIMUM_LENGTH { get; set; }
    public string IS_NULLABLE { get; set; }
    public object COLUMN_DEFAULT { get; set; }
    public string COLUMN_KEY { get; set; }
}