using System.Threading.Tasks;
using dapper_contrib_implementation;
using ef_base_repository;
using ef_dapper_models;
using ef_implementation_tests;
using ef_implementation;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class UserServiceTestsDapperContrib:BaseTest
{
    public IEFDataContext DbContext { get; set; }
    

    public UserServiceTestsDapperContrib()
    {
        this.DbContext = GetMySqlDbContext();
    }
    
    [Fact]
    public async Task Insert_Should_Add_User_To_Database()
    {
        // Arrange
        var service = new UserService_DapperContrib(DbContext);

        var user = new UserDapperContrib
        {
            FirstName = "John1",
            Email = "test@example.com",
            // Guid = "test",
            // LastName = "Test",
            // PhoneNumber = "074291773"
        };

        // Act
        var result = await service.Insert(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John1", result.FirstName);
        Assert.True(result.Id > 0);

        // Verify it was actually persisted
        // var savedUser = await service.FindById(result.Id);
        // Assert.NotNull(savedUser);
    }
    
}