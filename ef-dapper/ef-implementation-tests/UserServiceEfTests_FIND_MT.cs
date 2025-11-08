using System.Threading.Tasks;
using ef_base_repository;
using ef_dapper_models;
using ef_implementation_tests;
using ef_implementation;
using Microsoft.EntityFrameworkCore;
using Xunit;

public partial class UserServiceEfTests
{
    

    [Fact]
    public async Task EFGetUsersAndInvoicesMT()
    {
        // Arrange
        var dbContext = GetMySqlDbContext();
        var service = new UserServiceEF(dbContext);
        
        var filter = new QueryFilter
        {
            Conditions =
            {
                new QueryCondition { Field = "Id", Operator = SqlOperator.Equals, Value = 6 },
                // new QueryCondition { Field = "IsActive", Operator = SqlOperator.Equals, Value = true }
            }
        };
        var response = await service.GetUserInvoicesMT(filter);
        
        Assert.NotNull(response);

    }
    
    [Fact]
    public async Task GetUsers_InvoicesAsyncMultipleQueries()
    {
        // Arrange
        var dbContext = GetMySqlDbContext();
        var service = new UserServiceEF(dbContext);
        
        var filter = new QueryFilter
        {
            Conditions =
            {
                new QueryCondition { Field = "Id", Operator = SqlOperator.Equals, Value = 3 },
                // new QueryCondition { Field = "IsActive", Operator = SqlOperator.Equals, Value = true }
            }
        };
        var response = await service.GetUsers_InvoicesAsyncMultipleQueries(filter);
        
        Assert.NotNull(response);

    }
    
    
    
    
    
    
    
}