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
    public async Task FindUserAndInvoicesFromRawSQL()
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
        var result = await service.FindUserAndInvoicesFromRawSQL(filter);

        // Assert
        Assert.NotNull(result);

        // Verify it was actually persisted
    }
    
}