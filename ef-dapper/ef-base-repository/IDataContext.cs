using System.Data.Common;
using ef_dapper_models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ef_base_repository;
public interface IEFDataContext
{
    DbSet<T> Set<T>() where T : class, IRootEntity;
    DbConnection GetDbConnection(); 
    string ConnectionString { get; set; }
    DatabaseFacade Database { get; }
    EntityEntry Entry(object entity);
    ChangeTracker ChangeTracker { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}