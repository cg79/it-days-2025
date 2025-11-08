
using System.Data;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using ef_dapper_models;
using Efcom.Base.Repository.Request;
using Microsoft.EntityFrameworkCore;

namespace ef_base_repository;

public  class GenericRepository<T>(IEFDataContext context) : BaseRepository<T>(context), IGenericRepository<T>
    where T : class, IRootEntity
{
    protected readonly IEFDataContext _context = context;

    public Task<List<T>> Filter(string filterExpression)
    {
        var whereExpression = GenericExpression.CreateFilterExpression<T>(filterExpression);
        var source = _context.Set<T>()
               .Where(whereExpression).ToListAsync();
        return source;
    }
    
    public IQueryable<T> FindQueryFilter(QueryFilter filter)
    {
        var predicate = filter.ToExpression<T>();
        return _context.Set<T>().Where(predicate);
    }
    public IQueryable<T> FindLinq( Expression<Func<T,bool>> whereExpression)
    {
        return _context.Set<T>().Where(whereExpression);
    }
    
    public IQueryable<T> Filter(FilterGroup filterGroup)
    {
        string filterExpression = FilterParser.Parse(filterGroup);
        var whereExpression = GenericExpression.CreateFilterExpression<T>(filterExpression);
        return _context.Set<T>()
            .Where(whereExpression);
    }

    public async Task<PaginationResponse<T>> GetPagedAsync(
        PaginationRequest paginationRequest,
        CancellationToken cancellationToken = default)
    {
        var dbRecords = _context.Set<T>().AsQueryable<T>();
        var source = dbRecords;

        if (paginationRequest.FilterExpression != null)
        {
            var whereExpression = GenericExpression.CreateFilterExpression<T>(paginationRequest.FilterExpression);
            source = source.Where(whereExpression);
        }

        if (paginationRequest.FilterGroup != null)
        {
            var expression = FilterParser.Parse(paginationRequest.FilterGroup);
            var whereExpression = GenericExpression.CreateFilterExpression<T>(expression);
            source = source
                .Where(whereExpression);
        }

        if (paginationRequest.SortCriteria != null)
        {
            source = GenericExpression.ApplySorting(source, paginationRequest.SortCriteria);
        }

        var records = await source
            .Skip((paginationRequest.PageNo - 1) * paginationRequest.PageSize)
            .Take(paginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        var totalCount = await dbRecords.CountAsync(cancellationToken);
        return new PaginationResponse<T>
        {
            TotalCount = totalCount,
            Items = records
        };

    }

}
