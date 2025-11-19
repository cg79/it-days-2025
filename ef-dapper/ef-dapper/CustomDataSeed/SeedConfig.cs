namespace ef_dapper_CustomDataSeed;

public class SeedConfig
{
    public string table { get; set; }
    public int count { get; set; }
    public Dictionary<string, FieldRule>? fields { get; set; }
}

public class FieldRule
{
    public string? type { get; set; }          // faker | range | lookup
    public string method { get; set; }        // Name.FirstName, Internet.Email, ...
    public int min { get; set; }
    public int max { get; set; }
    public string table { get; set; }         // lookup table
    public string field { get; set; }         // lookup field
}