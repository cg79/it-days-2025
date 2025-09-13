using System.Data.Common;
using ef_dapper_models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ef_base_repository;
public interface IEFDataContext
{
    DbSet<T> Set<T>() where T : class, IRootEntity;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    // DatabaseFacade Database { get; }
    DbConnection GetDbConnection(); // just the signature
    string ConnectionString { get; set; }
    EntityEntry<T> EntryVal<T>(T entityEntryEntity) where T : class, IRootEntity;
}