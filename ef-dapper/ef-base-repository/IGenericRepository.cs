
using ef_dapper_models;
using Efcom.Base.Repository.Request;

namespace ef_base_repository;

public interface IGenericRepository<T> where T : IRootEntity
{
    Task<T?> FindByIdAsync(long id);
    Task DeleteByIdAsync(long id);
    Task<PaginationResponse<T>> GetPagedAsync(
        PaginationRequest paginationRequest,
        CancellationToken cancellationToken = default);

    Task<List<T>> Filter(string filterExpression);
}
