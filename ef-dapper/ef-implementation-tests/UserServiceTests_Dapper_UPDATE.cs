using System.Threading.Tasks;
using dapper_contrib_implementation;
using dapper_dommel_implementation;
using dapper_implementation;
using dapper_simple_crud_implementation;
using ef_base_repository;
using ef_dapper_models;
using ef_implementation_tests;
using ef_implementation;
using linq_to_db_implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using repodb_implementation;
using Xunit;

public partial class UserServiceTestsDapper
{
    [Fact]
    public async Task UpdateUser_Dapper_RawSql()
    {
        var service = new UserService_Dapper(this.DbContext);

        var user = new User
        {
            Id = 8,
            FirstName = "John",
            Email = "test@example.com"
        };

        try
        {
            var updated = await service.UpdateRawSql(user);
            Assert.True(updated>0);
        }
        catch (Exception ex)
        {
            Assert.True(false, ex.Message);
        }
    }


    [Fact]
    public async Task UpdateUser_Test_Dapper()
    {
        // Arrange
        var service = new UserService_Dapper(this.DbContext);

        var user = new
        {
            Id = 8,
            FirstName = "John",
            Email = "test@example.com"
        };

        // Act
        var updated = await service.UpdateObject(user);
        Assert.True(updated > 0);
    }
    
    [Fact]
    public async Task UpdateUser_Test_DapperContrib()
    {
        // Arrange
        var service = new UserService_DapperContrib(this.DbContext);

        var user = new
        {
            Id = 8,
            FirstName = "John",
            Email = "test@example.com"
        };

        // Act
        var updated = await service.Update(user);
        Assert.True(updated > 0);
    }
    
    [Fact]
    public async Task UpdateUser_Test_SimpleCrud()
    {
        // Arrange
        var service = new UserService_SimpleCrud(this.DbContext);

        var user = new
        {
            Id = 8,
            FirstName = "John",
            Email = "test@example.com"
        };

        // Act
        var updated = await service.Update(user);
        Assert.True(updated > 0);
    }
    
    [Fact]
    public async Task UpdateUser_Test_Dommel()
    {
        // Arrange
        var service = new UserService_Dommel(this.DbContext);

        var user = new
        {
            Id = 8,
            FirstName = "John",
            Email = "test@example.com"
        };

        // Act
        var updated = await service.Update(user);
        Assert.True(updated > 0);
    }

    
    [Fact]
    public async Task UpdateUser_Test_RepoDB()
    {
        // Arrange
        var service = new UserService_RepoDb(this.DbContext);

        var user = new
        {
            Id = 8,
            FirstName = "John",
            Email = "test@example.com"
        };

        // Act
        var updated = await service.Update(user);
        Assert.True(updated > 0);
    }
    
    [Fact]
    public async Task UpdateUser_Test_Linq2DB()
    {
        // Arrange
        var service = new UserService_LinqToDb(this.DbContext);

        var user = new
        {
            Id = 8,
            FirstName = "John",
            Email = "test@example.com"
        };

        // Act
        var updated = await service.Update(user);
        Assert.True(updated > 0);
    }

}