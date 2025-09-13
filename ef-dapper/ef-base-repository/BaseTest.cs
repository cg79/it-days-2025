using ef_base_repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ef_implementation_tests;

public class BaseTest
{
    protected string connectionString = "server=127.0.0.1;port=3306;database=MyAppDb;user=appuser;password=AppPass123;";
    
    
    protected IEFDataContext GetMySqlDbContext()
    {
        return CreateDataContext();
    }

    protected DataContext CreateDataContext()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseMySql(connectionString, new MySqlServerVersion(new Version(8, 3, 0)))
            .LogTo(Console.WriteLine, LogLevel.Information) // logs SQL
            // .EnableSensitiveDataLogging() 
            .Options;

        var context = new DataContext(options);

        // Ensure the schema exists
        context.Database.EnsureCreated();
        context.ConnectionString = connectionString;

        return context;
    }
}