// using ef_dapper_models;
// using Microsoft.EntityFrameworkCore;
// using System.Data.Common;
// using Microsoft.EntityFrameworkCore.ChangeTracking;
//
// namespace ef_base_repository
// {
//     public class BenchMarkDataContext : DbContext, IEFDataContext
//     {
//         public BenchMarkDataContext(DbContextOptions<BenchMarkDataContext> options) : base(options) { }
//         public  DbSet<User> Users { get; set; }
//         public  DbSet<Product> Products { get; set; }
//         public  DbSet<Invoice> Invoices { get; set; }
//         public  DbSet<InvoiceLine> InvoiceLines { get; set; }
//         public  DbSet<TimeBill> TimeBills { get; set; }
//         public  DbSet<Payment> Payments { get; set; }
//
//
//
//         public new DbSet<T> Set<T>() where T : class, IRootEntity
//         {
//             return base.Set<T>();
//         }
//
//         public DbConnection GetDbConnection()
//         {
//             return this.Database.GetDbConnection(); 
//         }
//
//         public string ConnectionString { get; set; }
//         public EntityEntry<T> EntryVal<T>(T entityEntryEntity) where T : class, IRootEntity
//         {
//             throw new NotImplementedException();
//         }
//     }
// }
