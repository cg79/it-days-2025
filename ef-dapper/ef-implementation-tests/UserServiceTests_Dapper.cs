using System.Threading.Tasks;
using ef_base_repository;
using ef_dapper_models;
using ef_implementation_tests;
using ef_implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

public class UserServiceTestsDapper: BaseTest
{
    public IEFDataContext DbContext { get; set; }
    
    public UserServiceTestsDapper()
    {
        this.DbContext = this.GetMySqlDbContext();
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
        var result = await service.Insert(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John", result.FirstName);

    }
    
    [Fact]
    public async Task Insert_Should_Add_User_To_Database_Dapper()
    {
        // Arrange
        var service = new UserService_Dapper(this.DbContext);

        var user = new User
        {
            FirstName = "John",
            Email = "test@example.com"
        };

        // Act
        var result = await service.Insert(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John", result.FirstName);

        // Verify it was actually persisted
        var savedUser = await DbContext.Set<User>().FindAsync(result.Id);
        Assert.NotNull(savedUser);
    }
    

    private async Task<WeakReference> CreateServiceAndInsertAsync(int i)
    {
        await using var context = CreateDataContext();
        var service = new UserService_Dapper(context);
        var user = new User { Email = $"leaktest{i}@example.com" };
        await service.Insert(user);

        return new WeakReference(service);
    }
    
    
    [Fact]
    public async Task DetectMemoryLeak()
    {
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
    
    [Fact]
    public async Task FindUsers_Should_Return_Values()
    {
        // Arrange
        var dbContext = GetMySqlDbContext();
        var service = new UserService_Dapper(dbContext);


        var filter = new QueryFilter
        {
            Conditions =
            {
                new QueryCondition { Field = "FirstName", Operator = SqlOperator.Contains, Value = "Join" },
                new QueryCondition { Field = "Age", Operator = SqlOperator.GreaterOrEqual, Value = 18 },
                // new QueryCondition { Field = "IsActive", Operator = SqlOperator.Equals, Value = true }
            }
        };

        // Act
        var result = await service.Find(filter);

        // Assert
        Assert.NotNull(result);

        // Verify it was actually persisted
    }
    
    
    [Fact]
    public async Task FilterUsers_Should_Return_Filtered_Users()
    {
        // Arrange
        var dbContext = GetMySqlDbContext();
        var service = new UserService_Dapper(dbContext);


        var filter = new FilterGroup
        {
            Logic = "and",
            Filters = new List<object>
            {
                new FilterCondition { Field = "Age", Operator = FilterOperator.Gt, Value = 10 },
                new FilterGroup
                {
                    Logic = "or",
                    Filters = new List<object>
                    {
                        new FilterCondition { Field = "Email", Operator = FilterOperator.Contains, Value = "gmail.com" },
                        new FilterCondition { Field = "Email", Operator = FilterOperator.Contains, Value = "yahoo.com" }
                    }
                }
            }
        };

        // Act
        var result = await service.Filter(filter);

        // Assert
        Assert.NotNull(result);

        // Verify it was actually persisted
    }

}