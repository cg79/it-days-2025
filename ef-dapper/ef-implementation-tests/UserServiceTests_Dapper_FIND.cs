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
    public async Task FindUsers_WITHSP_Should_Return_Values()
    {
        // Arrange
        var dbContext = GetMySqlDbContext();
        var service = new UserService_Dapper(dbContext);


        // Act
        var result = await service.FindWithSP();

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
            Filters =
            [
                new FilterCondition { Field = "Age", Operator = FilterOperator.Gt, Value = 10 },
                new FilterGroup
                {
                    Logic = "or",
                    Filters =
                    [
                        new FilterCondition
                        {
                            Field = "Email", Operator = FilterOperator.Contains, Value = "gmail.com"
                        },

                        new FilterCondition
                        {
                            Field = "Email", Operator = FilterOperator.Contains, Value = "yahoo.com"
                        }
                    ]
                }
            ]
        };

        // Act
        var result = await service.Filter(filter);

        // Assert
        Assert.NotNull(result);

        // Verify it was actually persisted
    }

   
}