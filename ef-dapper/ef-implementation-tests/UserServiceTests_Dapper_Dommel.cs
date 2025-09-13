using dapper_dommel_implementation;
using ef_base_repository;
using ef_dapper_models;
using ef_implementation_tests;
using Xunit;

public class UserServiceTestsDapperDommel:BaseTest
{
    public IEFDataContext DbContext { get; set; }
    

    public UserServiceTestsDapperDommel()
    {
        this.DbContext = GetMySqlDbContext();
    }
    
    [Fact]
    public async Task Insert_Should_Add_User_To_Database()
    {
        // Arrange
        var service = new UserService_Dommel(DbContext);

        var user = new User
        {
            FirstName = "John1",
            Email = "test@example.com",
            Guid = "test",
            LastName = "Test",
            PhoneNumber = "074291773"
        };

        // Act
        var result = await service.Insert(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John1", result.FirstName);
        Assert.True(result.Id > 0);

    }
    
}