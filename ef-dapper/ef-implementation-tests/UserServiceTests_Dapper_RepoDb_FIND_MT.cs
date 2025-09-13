using System.Threading.Tasks;
using ef_base_repository;
using ef_dapper_models;
using ef_implementation_tests;
using ef_implementation;
using Microsoft.EntityFrameworkCore;
using repodb_implementation;
using RepoDb;
using Xunit;

public partial class UserServiceTestsDapperExtensions
{
    
    [Fact]
    public async Task FIndMt()
    {
        var filter = new QueryFilter
        {
            Conditions =
            {
                new QueryCondition { Field = "Id", Operator = SqlOperator.Equals, Value = 6 },
                // new QueryCondition { Field = "IsActive", Operator = SqlOperator.Equals, Value = true }
            }
        };
        // Arrange
        var service = new UserService_RepoDb(DbContext);
        var result = await service.GetUserInvoicesMT(filter);
        // // Assert
        Assert.NotNull(result);

    }
    
}