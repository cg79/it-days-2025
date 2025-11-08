using dapper_dommel_implementation;
using ef_base_repository;
using ef_dapper_models;
using ef_implementation_tests;
using Efcom.Base.Repository.Request;
using Xunit;

public class UserServiceTestsDommelPagination:BaseTest
{
    public IEFDataContext DbContext { get; set; }
    

    public UserServiceTestsDommelPagination()
    {
        this.DbContext = GetMySqlDbContext();
    }
    
    [Fact]
    public async Task DommelPaginationWithFilterExpression()
    {
        // Arrange
        var service = new UserService_Dommel(DbContext);

        var paginationRequest = new PaginationRequest()
        {
            PageNo = 1,
            PageSize = 7,
            SortCriteria = new Dictionary<string, bool>
            {
                { "PhoneNumber", false },
                { "FirstName", true }
            },
            FilterExpression = "Age >34 AND FirstName LIKE '%Asia%'", 
        };
        // Act
        var result = await service.Pagination(paginationRequest);

        // Assert
        Assert.NotNull(result);

    }
    
    [Fact]
    public async Task DommelPaginationWithFilterGroup()
    {
        // Arrange
        var service = new UserService_Dommel(DbContext);

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
        var result = await service.Pagination(paginationRequest);

        // Assert
        Assert.NotNull(result);

    }
    
}