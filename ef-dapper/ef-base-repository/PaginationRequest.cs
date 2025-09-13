using ef_base_repository;

namespace Efcom.Base.Repository.Request;

public class PaginationRequest
{
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public string? FilterExpression { get; set; }
    
    public FilterGroup FilterGroup { get; set; }
}
