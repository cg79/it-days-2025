using ef_base_repository;
using ef_dapper_models;
using ef.Cryptography;
using Efcom.Base.Repository.Request;
using LinqToDB.Common.Internal.Cache;
using Microsoft.Extensions.Caching.Memory;

namespace ef_cache;

public class CachingRepositoryDecorator<T>(IGenericRepository<T> inner, ICacheService cache) : IGenericRepository<T>
    where T : IRootEntity
{
    public async Task<T?> FindByIdAsync(long id)
    {
        var cacheKey = $"{typeof(T).Name}_Id_{id}";
        if (cache.TryGetValue(cacheKey, out T? entity))
            return entity;

        entity = await inner.FindByIdAsync(id);
        cache.Set(cacheKey, entity, TimeSpan.FromMinutes(5));
        return entity;
    }

    public Task DeleteByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<PaginationResponse<T>> GetPagedAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<T>> Filter(string filterExpression)
    {
        throw new NotImplementedException();
    }
}

internal interface IMemoryCache
{
}
