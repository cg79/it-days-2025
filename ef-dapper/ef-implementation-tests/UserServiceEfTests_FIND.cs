using System.Threading.Tasks;
using ef_base_repository;
using ef_dapper_models;
using ef_implementation_tests;
using ef_implementation;
using Microsoft.EntityFrameworkCore;
using Xunit;

public partial class UserServiceEfTests: BaseTest
{
    
    
    [Fact]
    public async Task FindUsers_Should_Return_Values()
    {
        // Arrange
        var dbContext = GetMySqlDbContext();
        var service = new UserServiceEF(dbContext);

        // Act
        var result = await service.FindWithLinq(el=>el.FirstName.Contains("Asia") && el.Age >= 18).ToListAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count()>0);

        // Verify it was actually persisted
    }
    
    
    [Fact]
    public async Task FindInvoicesWithMultipleQueries_Should_Return_Values()
    {
        // Arrange
        var dbContext = GetMySqlDbContext();
        var service = new UserServiceEF(dbContext);


        var filter = new QueryFilter
        {
            Conditions =
            {
                new QueryCondition { Field = "UserId", Operator = SqlOperator.Equals, Value = 6 },
                // new QueryCondition { Field = "IsActive", Operator = SqlOperator.Equals, Value = true }
            }
        };

        // Act
        var result = await service.getInvoicesWithMultipleQueries(filter);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count()>0);

        // Verify it was actually persisted
    }
    
    [Fact]
    public async Task FilterUsers_Should_Return_Filtered_Users()
    {
        // Arrange
        var dbContext = GetMySqlDbContext();
        var service = new UserServiceEF(dbContext);

        var user = new User
        {
            FirstName = "John",
            Email = "test@example.com"
        };

        // string filterExpression = "Id != 11";
        var keyword = "Allen";

        var group = new FilterGroup
        {
            Logic = "and",
            Filters = new List<object>
            {
                new FilterCondition { Field = "Id", Operator = FilterOperator.Gt, Value = 10 },
                new FilterGroup
                {
                    Logic = "or",
                    Filters = new List<object>
                    {
                        new FilterCondition { Field = "Email", Operator = FilterOperator.Contains, Value = "Verona" },
                        new FilterCondition { Field = "Email", Operator = FilterOperator.Contains, Value = "Price" }
                    }
                }
            }
        };
        // Act
        var result = await service.Filter(group);

        // Assert
        Assert.NotNull(result);

        // Verify it was actually persisted
    }
    
}