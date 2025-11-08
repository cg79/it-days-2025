using System.Threading.Tasks;
using dapper_simple_crud_implementation;
using ef_base_repository;
using ef_dapper_models;
using ef_implementation_tests;
using ef_implementation;
using Microsoft.EntityFrameworkCore;
using Xunit;

public partial class UserServiceTestsSimpleCrud:BaseTest
{
    public IEFDataContext DbContext { get; set; }
    public UserServiceTestsSimpleCrud()
    {
        this.DbContext = GetMySqlDbContext();
    }
    
    [Fact]
    public async Task Insert_Should_Add_User_To_Database()
    {
        // Arrange
        var service = new UserService_SimpleCrud(DbContext);

        var user = new User
        {
            FirstName = "John",
            Email = "test@example.com"
        };

        // Act
        var result = await service.Insert(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John", result.FirstName);
        Assert.True(result.Id > 0);

        // Verify it was actually persisted
        // var savedUser = await service.FindById(result.Id);
        // Assert.NotNull(savedUser);
    }
    
}