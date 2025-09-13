using System.Threading.Tasks;
using ef_base_repository;
using ef_dapper_models;
using ef_implementation_tests;
using ef_implementation;
using Microsoft.EntityFrameworkCore;
using repodb_implementation;
using RepoDb;
using Xunit;

public partial class UserServiceTestsDapperExtensions
{
    
    [Fact]
    public async Task FInd()
    {
        // Arrange
        var service = new UserService_RepoDb(DbContext);
        var result = await service.FindLinq(el=>el.FirstName.Contains("Asia") && el.Age >= 18);
        // // Assert
        Assert.NotNull(result);

    }
    
}