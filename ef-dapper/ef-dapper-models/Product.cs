namespace ef_dapper_models;

public class Products: IRootEntity
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    
}