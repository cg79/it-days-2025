using System.Linq.Expressions;
using ef_dapper_models;
using Microsoft.EntityFrameworkCore;

namespace ef_base_repository;

public  class BaseRepository<T>(IEFDataContext context) 
    where T : class, IRootEntity
{
    public async Task<T> InsertAsync(T entity, bool tracking = true)
    {
        var entityEntry = await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
        if (!tracking)
        {
            context.EntryVal(entityEntry.Entity).State = EntityState.Detached;
        }
        return entityEntry.Entity;
    }
    
    public  Task<T?> GetByIdAsync(int id)
    {
        var entity =  context.Set<T>().FirstOrDefault(e => e.Id == id);
        return Task.FromResult(entity);
    }

    public async Task DeleteByIdAsync(int id)
    {
        var records = context.Set<T>();
        var user = await records.FindAsync(id);
        if (user != null)
        {
            records.Remove(user);
            await context.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(int id, string notFoundMessage, Action<T> updateProperties, CancellationToken cancellationToken )
    {
        var entity =  await GetByIdAsync(id);
        if (entity == null)
        {
            throw new Exception(notFoundMessage);
        }
        
        updateProperties(entity);
        context.Set<T>().Update(entity);
        await context.SaveChangesAsync(cancellationToken);
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

    // private async UpdateEntity(T entity, CancellationToken cancellationToken )
    // {
    //     context.Set<T>().Update(entity);
    //     await context.SaveChangesAsync(cancellationToken);
    // }
    
}
