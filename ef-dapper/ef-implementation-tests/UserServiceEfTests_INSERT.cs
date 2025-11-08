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
    public async Task Insert_Should_Add_User_To_Database()
    {
        // Arrange
        var dbContext = GetMySqlDbContext();
        var service = new UserServiceEF(dbContext);

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

        // Verify it was actually persisted
        var savedUser = await dbContext.Set<User>().FindAsync(result.Id);
        Assert.NotNull(savedUser);
    }
    
    
    [Fact]
    public async Task Insert_Should_Add_User_To_DatabaseNoTracking()
    {
        // Arrange
        var dbContext = GetMySqlDbContext();
        var service = new UserServiceEF(dbContext);

        var user = new User
        {
            FirstName = "John",
            Email = "test@example.com"
        };

        // Act
        var result = await service.InsertNoTracking(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John", result.FirstName);

        // Verify it was actually persisted
        var savedUser = await dbContext.Set<User>().FindAsync(result.Id);
        Assert.NotNull(savedUser);
    }
    
    
    
}