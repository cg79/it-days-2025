using System.Dynamic;
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
    public async Task EF_UpdateUser_WithDelegate_Tracked_1()
    {
        var fn = Guid.NewGuid().ToString();
        var dbContext = GetMySqlDbContext();
        var service = new UserServiceEF(dbContext);
        var updated = await service.UpdateByIdAsyncTRACKED(6,
                u => { u.FirstName = fn; },
                new CancellationToken());

        // Assert
        Assert.True(updated>0);
    }

    [Fact]
    public async Task EF_UpdateUser_WithEntity_DETACHED_2()
    {
        // Arrange
        var dbContext = GetMySqlDbContext();
        var service = new UserServiceEF(dbContext);

        for (var i = 0; i < 5; i++)
        {
            var fn = Guid.NewGuid().ToString();
            // var user = new User
            // {
            //     Id = 7,
            //     FirstName = fn,
            //     Email = "test@example.com"
            // };

            // Act
            var updated = await service.UpdateByIdAsyncUNTRACKED(
                7,
                u => u.FirstName = fn,
                new CancellationToken());
            
            Assert.True(updated > 0);
        }
    }

    [Fact]
    public async Task EF_UpdateUser_CreateRawSQL_3()
    {
        // Arrange
        var dbContext = GetMySqlDbContext();
        var userServiceEF = new UserServiceEF(dbContext);
        var fn = Guid.NewGuid().ToString();

        var user = new
        {
            Id = 7,
            FirstName = fn,
            Email = "test@example.com"
        };
        var updated = await userServiceEF.UpdateByIdUsingEntityAndRawSql(
            user,
            "Users",
            new CancellationToken());
        Assert.True(updated > 0);
    }




}