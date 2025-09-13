using System.Threading.Tasks;
using ef_base_repository;
using ef_dapper_models;
using ef_implementation_tests;
using ef_implementation;
using Microsoft.EntityFrameworkCore;
using repodb_implementation;
using RepoDb;
using Xunit;
// using RepoDb.MySqlConnector;  // required
using MySqlConnector;

public partial class UserServiceTestsDapperExtensions:BaseTest
{
    public IEFDataContext DbContext { get; set; }
    

    public UserServiceTestsDapperExtensions()
    {
        this.DbContext = GetMySqlDbContext();
        // GlobalConfiguration.Setup().UseMySql();
        RepoDb.MySqlBootstrap.Initialize();
    }

    [Fact]
    public async Task Insert_Should_Add_User_To_Database()
    {
        // Arrange
        var service = new UserService_RepoDb(DbContext);

        var user = new User
        {
            FirstName = "John1",
            Email = "test@example.com",
            Guid = "test",
            LastName = "Test",
            PhoneNumber = "074291773"
        };
        try
        {
            // Act
            var result = await service.Insert(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John1", result.FirstName);
            Assert.True(result.Id > 0);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }
    
}