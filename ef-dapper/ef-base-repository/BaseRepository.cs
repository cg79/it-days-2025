using System.Data;
using System.Linq.Expressions;
using ef_dapper_models;
using Microsoft.EntityFrameworkCore;

namespace ef_base_repository;

public  class BaseRepository<T>(IEFDataContext context) 
    where T : class, IRootEntity
{
    public async Task<T> InsertAsync(T entity)
    {
        var entityEntry = await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
        
        return entityEntry.Entity;
    }
    public async Task<T> InsertAsyncNoTracking(T entity)
    {
        var entityEntry = await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
            context.Entry(entityEntry.Entity).State = EntityState.Detached;
        return entityEntry.Entity;
    }
    
    public async Task<T?> FindByIdAsync(long id)
    {
        return  await context.Set<T>().FindAsync(id);
    }

    public async Task DeleteByIdAsync(long id)
    {
        var records = context.Set<T>();
        var user = await records.FindAsync(id);
        if (user != null)
        {
            records.Remove(user);
            await context.SaveChangesAsync();
        }
    }

    public async Task<int> UpdateByIdAsyncTRACKED(long id, Action<T> updateProperties, CancellationToken cancellationToken )
    {
        var entity =  await FindByIdAsync(id);
        if (entity == null)
        {
            return 0;
        }
        updateProperties(entity);
        return await context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<int> UpdateByIdAsyncUNTRACKED(
        long id,
        Action<T> updateProperties,
        CancellationToken cancellationToken = default)
    {
        var entity = Activator.CreateInstance<T>();
        typeof(T).GetProperty("Id")?.SetValue(entity, id);
        
        // var tracked = context.ChangeTracker.Entries<T>()
        //     .FirstOrDefault(e => e.Entity.Id == id);
        //
        // if (tracked != null)
        //     context.Entry(tracked.Entity).State = EntityState.Detached;
        //
        context.Set<T>().Attach(entity);
        updateProperties(entity);

        foreach (var prop in context.Entry(entity).Properties)
        {
            if (!prop.Metadata.IsPrimaryKey() && prop.CurrentValue != null)
            {
                prop.IsModified = true;
            }
        }

        var updated = await context.SaveChangesAsync(cancellationToken);
        context.Entry(entity).State = EntityState.Detached;
        return updated;
    }

    
    public async Task UpdateAsync(Expression<Func<T, bool>> predicate, string notFoundMessage, Action<T> updateProperties, CancellationToken cancellationToken)
    {
        var entity = await context.Set<T>().SingleOrDefaultAsync(predicate, cancellationToken);
    
        if (entity == null)
        {
            throw new KeyNotFoundException(notFoundMessage);
        }

        updateProperties(entity);

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task ExecuteRawSqlAsync(string sql, object entity, CancellationToken cancellationToken = default)
    {
        await context.Database.ExecuteSqlRawAsync(sql, entity, cancellationToken);
    }
    
    public async Task<int> UpdateByIdUsingEntityAndRawSql(object entity, string tableName, CancellationToken cancellationToken = default)
    {
        var formattableSql = GenericExpression.CreateFormattableUpdateQuery(entity, tableName);
        return await context.Database.ExecuteSqlInterpolatedAsync(formattableSql, cancellationToken);
    }
    
    
}
