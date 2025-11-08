using System.Threading.Tasks;
using ef_base_repository;
using ef_dapper_models;
using ef_implementation_tests;
using ef_implementation;
using linq_to_db_implementation;
using Microsoft.EntityFrameworkCore;
using Xunit;

public partial class UserServiceTestsLinqToDb: BaseTest
{
    public IEFDataContext DbContext { get; set; }

    public UserService_LinqToDb CreateService()
    {
        var dbContext = GetMySqlDbContext();
        var service = new UserService_LinqToDb(dbContext);
        return service;
    }

    public UserServiceTestsLinqToDb()
    {
        this.DbContext = this.GetMySqlDbContext();
        // var x = new l2dbData.DataConnection
    }
    
    [Fact]
    public async Task Insert_Should_Add_User_To_Database()
    {
        // Arrange
        var service = new UserService_LinqToDb(DbContext);

        var user = new User()
        {
            FirstName = "John",
            Email = "testLinqTodb@example.com"
        };

        // Act
        var result = await service.Insert(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John", result.FirstName);
        Assert.True(result.Id > 1);

        // Verify it was actually persisted
        // var savedUser = await service.FindById(result.Id);
        // Assert.NotNull(savedUser);
    }
    
    [Fact]
    public async Task FilterUsers_Should_Return_Filtered_UsersWithCustomFileds()
    {
        // Arrange
        var dbContext = GetMySqlDbContext();
        var service = new UserService_LinqToDb(dbContext);


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
        var result = await service.FilterWithCustomFileds(filter);

        // Assert
        Assert.NotNull(result);

        // Verify it was actually persisted
    }
    
}