using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using dapper_simple_crud_implementation;
using ef_base_repository;
using ef_dapper_models;
using ef_implementation_tests;
using ef_implementation;
using Microsoft.EntityFrameworkCore;
using Xunit;



public partial class UserServiceTestsSimpleCrud
{
    [Fact]
    public async Task UpdateUserSimpleCrud_Object()
    {
        // Arrange
        var service = new UserService_SimpleCrud(DbContext);

        var fn = Guid.NewGuid().ToString();
        var user = new 
        {
            Id = 9,
            FirstName = fn,
            Email = "test@example.com"
        };

        // Act
        var result = await service.UpdateObject(user);

        // Assert
        Assert.NotNull(result);
        // Assert.Equal("John", result.FirstName);
        // Assert.True(result.Id > 0);

        // Verify it was actually persisted
        // var savedUser = await service.FindById(result.Id);
        // Assert.NotNull(savedUser);
    }
    
    [Fact]
    public async Task UpdateUserSimpleCrud_TypeSafe()
    {
        // Arrange
        var service = new UserService_SimpleCrud(DbContext);

        var fn = Guid.NewGuid().ToString();
        var user = new UserToUpdate
        {
            Id = 10,
            FirstName = fn,
            Email = "test@example.com"
        };

        // Act
        var result = await service.UpdateObject(user);

        // Assert
        Assert.NotNull(result);
        // Assert.Equal("John", result.FirstName);
        // Assert.True(result.Id > 0);

        // Verify it was actually persisted
        // var savedUser = await service.FindById(result.Id);
        // Assert.NotNull(savedUser);
    }
    
}