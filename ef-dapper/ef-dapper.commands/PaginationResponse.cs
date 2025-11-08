namespace ef_dapper.commands;

public class PaginationResponse<T>
{
    public required List<T> Items { get; set; }
    public int TotalCount { get; set; }
}