
namespace ef_base_repository;

public class PaginationResponse<T>
{
    public required List<T> Items { get; set; }
    public int TotalCount { get; set; }
}