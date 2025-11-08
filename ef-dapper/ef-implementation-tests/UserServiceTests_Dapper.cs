using System.Threading.Tasks;
using dapper_implementation;
using ef_base_repository;
using ef_dapper_models;
using ef_implementation_tests;
using ef_implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

public partial class UserServiceTestsDapper: BaseTest
{
    public IEFDataContext DbContext { get; set; }
    
    public UserServiceTestsDapper()
    {
        this.DbContext = this.GetMySqlDbContext();
    }


    [Fact]
    public async Task FindProductsWithSP()
    {
        var service = new ProductService_Dapper(this.DbContext);
        var result = await service.FindProductsWithSP("Rustic", 30);
        
        // Assert
        Assert.NotNull(result);

    }

    [Fact]
    public async Task InsertMySQlConnection()
    {
        var service = new UserService_Dapper(this.DbContext);
        var user = new User()
        {
            Email = "admin@example.com",
        };
        var result = await service.InsertRawWithSqlConnection(user);
        Assert.NotNull(result);
    }

    

    [Fact]
    public async Task Insert_Should_Add_User_To_Database()
    {
        // Arrange
        var service = new UserService_Dapper(this.DbContext);

        var user = new User
        {
            FirstName = "John",
            Email = "test@example.com"
        };

        // Act
        var result = await service.InsertRawSql(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John", result.FirstName);

    }
    
    [Fact]
    public async Task Insert_WITHSPShould_Add_User_To_Database()
    {
        // Arrange
        var service = new UserService_Dapper(this.DbContext);

        var user = new User
        {
            FirstName = "John",
            Email = "test@example.com"
        };

        // Act
        var result = await service.InsertWithSP("claudiu9379@yahoo.com", "and");

        // Assert
        Assert.True(result>0);

    }
    
    [Fact]
    public async Task Insert_Should_Add_User_To_Database_Dapper()
    {
        // Arrange
        var service = new UserService_Dapper(this.DbContext);

        var user = new 
        {
            FirstName = "xxxxxxx2",
            Email = "test@example.com"
        };
        
        // Act
        var result = await service.InsertObject(user);
        Assert.True(result>0);

        // Assert
        // Assert.NotNull(result);
        // Assert.Equal("John", result.FirstName);
        //
        // // Verify it was actually persisted
        // var savedUser = await service.FindById(result.Id);
        // Assert.NotNull(savedUser);
    }
    

    private async Task<WeakReference> CreateServiceAndInsertAsync(int i)
    {
        await using var context = CreateDataContext();
        var service = new UserService_Dapper(context);
        var user = new User { Email = $"leaktest{i}@example.com" };
        await service.InsertRawSql(user);

        return new WeakReference(service);
    }
    
    
    [Fact]
    public async Task DetectMemoryLeak()
    {
        return;
        var weakRefs = new List<WeakReference>();
        for (int i = 0; i < 100; i++)
        {
            weakRefs.Add(await CreateServiceAndInsertAsync(i));
        }
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        var activeRefs = weakRefs.Where(wr => wr.IsAlive).ToList();
        Assert.True(
            activeRefs.Count() <= 1,
            activeRefs.Count().ToString()
        );
    }
    
    

}