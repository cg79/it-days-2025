namespace ef_dapper.commands;

public class PaginationRequest
{
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public string? FilterExpression { get; set; }
}