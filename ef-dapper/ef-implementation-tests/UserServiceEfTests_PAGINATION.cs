using System.Threading.Tasks;
using ef_base_repository;
using ef_dapper_models;
using ef_implementation_tests;
using ef_implementation;
using Efcom.Base.Repository.Request;
using Microsoft.EntityFrameworkCore;
using Xunit;

public partial class UserServiceEfTests: BaseTest
{
   
    [Fact]
    public async Task PaginatedUsersWithExpressionTest()
    {
        // Arrange
        var dbContext = GetMySqlDbContext();
        var service = new UserServiceEF(dbContext);

        var paginationRequest = new PaginationRequest()
        {
            PageNo = 1,
            PageSize = 7,
            SortCriteria = new Dictionary<string, bool>
            {
                { "PhoneNumber", false },
                { "FirstName", true }
            },
            FilterExpression = "Age >34 && FirstName.Contains(\"Asia\")", 
        };
        // Act
        var result = await service.PaginatedUsers(paginationRequest);

        // Assert
        Assert.NotNull(result);
    }    
 
    [Fact]
    public async Task PaginatedUsersWithFilterCriteriaTest()
    {
        // Arrange
        var dbContext = GetMySqlDbContext();
        var service = new UserServiceEF(dbContext);

        var paginationRequest = new PaginationRequest()
        {
            PageNo = 1,
            PageSize = 7,
            SortCriteria = new Dictionary<string, bool>
            {
                { "PhoneNumber", false },
                { "FirstName", true }
            },
            FilterGroup = new FilterGroup
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
            }
        };
        

        // Act
        var result = await service.PaginatedUsers(paginationRequest);

        // Assert
        Assert.NotNull(result);
    }    
}