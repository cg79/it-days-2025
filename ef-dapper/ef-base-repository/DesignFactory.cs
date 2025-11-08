using ef_implementation_tests;


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ef_base_repository
{
    public class DataContextFactory : BaseTest, IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            return CreateDataContext();
            // var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            //
            // // ⚠️ adjust for your database
            // optionsBuilder.UseMySql(
            //     "server=localhost;database=ef_base_repository;user=root;password=yourpw",
            //     new MySqlServerVersion(new Version(8, 0, 36))
            // );
            //
            // return new DataContext(optionsBuilder.Options);
        }
    }
}
