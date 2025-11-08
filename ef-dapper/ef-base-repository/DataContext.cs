using ef_dapper_models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using LinqToDB.Reflection;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ef_base_repository
{
      public class DataContext : DbContext, IEFDataContext
      {
            public DataContext(DbContextOptions<DataContext> options) : base(options)
            {
            }

            public DbSet<User> Users { get; set; }
            public DbSet<Products> Products { get; set; }
            public DbSet<Invoice> Invoices { get; set; }
            public DbSet<InvoiceLine> InvoiceLines { get; set; }
            public DbSet<TimeBill> TimeBills { get; set; }
            public DbSet<Payment> Payments { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                  Console.WriteLine("OOOn model creating");
                  base.OnModelCreating(modelBuilder);

                  // -------------------------
                  // User
                  // -------------------------
                  modelBuilder.Entity<User>(entity =>
                  {
                        entity.ToTable("Users");

                        entity.HasKey(u => u.Id);

                        entity.Property(u => u.FirstName)
                              .HasMaxLength(100);

                        entity.Property(u => u.LastName)
                              .HasMaxLength(100);

                        entity.Property(u => u.Email)
                              .IsRequired()
                              .HasMaxLength(200);

                        entity.Property(u => u.PhoneNumber)
                              .HasMaxLength(30);

                        entity.Property(u => u.Guid)
                              .HasMaxLength(36);

                        // One-to-Many: User -> Invoices
                        entity.HasMany(u => u.Invoices)
                              .WithOne(i => i.User)
                              .HasForeignKey(i => i.UserId)
                              .OnDelete(DeleteBehavior.Cascade);
                  });

                  // -------------------------
                  // Invoice
                  // -------------------------
                  modelBuilder.Entity<Invoice>(entity =>
                  {
                        entity.ToTable("Invoices");

                        entity.HasKey(i => i.Id);

                        entity.Property(i => i.InvoiceDate)
                              .IsRequired();

                        entity.Property(i => i.Status)
                              .IsRequired()
                              .HasMaxLength(50);

                        entity.Property(i => i.TotalAmount)
                              .HasColumnType("decimal(18,2)");

                        // One-to-Many: Invoice -> InvoiceLines
                        entity.HasMany(i => i.InvoiceLines)
                              .WithOne(l => l.Invoice)
                              .HasForeignKey(l => l.InvoiceId)
                              .OnDelete(DeleteBehavior.Cascade);

                        // One-to-Many: Invoice -> TimeBills
                        entity.HasMany(i => i.TimeBills)
                              .WithOne(tb => tb.Invoice)
                              .HasForeignKey(tb => tb.InvoiceId)
                              .OnDelete(DeleteBehavior.Cascade);

                        // One-to-One: Invoice -> Payment
                        entity.HasOne(i => i.Payment)
                              .WithOne(p => p.Invoice)
                              .HasForeignKey<Payment>(p => p.InvoiceId)
                              .OnDelete(DeleteBehavior.Cascade);
                  });

                  // -------------------------
                  // InvoiceLine
                  // -------------------------
                  modelBuilder.Entity<InvoiceLine>(entity =>
                  {
                        entity.ToTable("InvoiceLines");

                        entity.HasKey(l => l.Id);

                        entity.Property(l => l.Description)
                              .IsRequired()
                              .HasMaxLength(200);

                        entity.Property(l => l.Quantity)
                              .IsRequired();

                        entity.Property(l => l.UnitPrice)
                              .HasColumnType("decimal(18,2)");
                  });

                  // -------------------------
                  // TimeBill
                  // -------------------------
                  modelBuilder.Entity<TimeBill>(entity =>
                  {
                        entity.ToTable("TimeBills");

                        entity.HasKey(tb => tb.Id);

                        entity.Property(tb => tb.WorkDate)
                              .IsRequired();

                        entity.Property(tb => tb.Hours)
                              .HasColumnType("decimal(5,2)");

                        entity.Property(tb => tb.RatePerHour)
                              .HasColumnType("decimal(18,2)");
                  });

                  // -------------------------
                  // Payment
                  // -------------------------
                  modelBuilder.Entity<Payment>(entity =>
                  {
                        entity.ToTable("Payments");

                        entity.HasKey(p => p.Id);

                        entity.Property(p => p.PaymentDate)
                              .IsRequired();

                        entity.Property(p => p.Amount)
                              .HasColumnType("decimal(18,2)");

                        entity.Property(p => p.Method)
                              .HasMaxLength(50);
                  });
            }


            public new DbSet<T> Set<T>() where T : class, IRootEntity
            {
                  return base.Set<T>();
            }

            public DbConnection GetDbConnection()
            {
                  // return this.Database.GetDbConnection();
                  return new MySqlConnector.MySqlConnection(this.Database.GetConnectionString());
            }

            // public DatabaseFacade DB => this.Database;

            public string ConnectionString { get; set; }
            // public EntityEntry<T> EntryVal<T>(T entityEntryEntity) where T : class, IRootEntity
            // {
            //       return this.Entry(entityEntryEntity);
            // }
      }
}
