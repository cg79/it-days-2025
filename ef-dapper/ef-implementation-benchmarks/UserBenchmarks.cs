using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;

using ef_base_repository;
using ef_dapper_models;
using ef_implementation_tests;
using ef_implementation;

[MemoryDiagnoser] 
public class UserBenchmarks: BaseTest
{
    private UserServiceEF _userServiceEf;
    private UserService_Dapper _userServiceDapper;
    private UserService_DapperContrib _userServiceContrib;
    private UserService_Dommel _userServiceDommel;
    private UserService_RepoDb _userServiceRepoDb;
    private UserService_LinqToDb _userServiceLinqToDb;
    private UserService_SimpleCrud _userServiceSimpleCrud;
    
    // private string _connectionString = "server=127.0.0.1;port=3306;database=MyAppDb;user=appuser;password=AppPass123;";
    // private IEFDataContext GetMySqlDbContext()
    // {
    //     var connectionString = "server=127.0.0.1;port=3306;database=MyAppDb;user=appuser;password=AppPass123;";
    //
    //     var options = new DbContextOptionsBuilder<BenchMarkDataContext>()
    //         .UseMySql(connectionString, new MySqlServerVersion(new Version(8, 3, 0)))
    //         .Options;
    //
    //     var context = new BenchMarkDataContext(options);
    //     context.ConnectionString = connectionString;
    //
    //     return context;
    // }

    [GlobalSetup]
    public void Setup()
    {
        // var options = new DbContextOptionsBuilder<BenchMarkDataContext>()
        //     .UseMySql(_connectionString, new MySqlServerVersion(new Version(8,3,0)))
        //     .Options;
        
        var dbContext = GetMySqlDbContext();

        _userServiceEf = new UserServiceEF(dbContext);
        _userServiceDapper = new UserService_Dapper(dbContext);
        _userServiceContrib = new UserService_DapperContrib(dbContext);
        _userServiceDommel = new UserService_Dommel(dbContext);
        _userServiceRepoDb = new UserService_RepoDb(dbContext);
        _userServiceLinqToDb = new UserService_LinqToDb(dbContext);
        _userServiceSimpleCrud = new UserService_SimpleCrud(dbContext);
    }

    [Benchmark]
    public async Task EfCore_Insert()
    {
        try
        {
            var user = new User { Email = "ef@test.com" };
            await _userServiceEf.Insert(user);
        }
        catch (Exception ex)
        {
            Console.WriteLine("ex1");
            Console.WriteLine(ex);
            throw;
        }
    }
    
    [Benchmark]
    public async Task EfCore_InsertNOTRACKING()
    {
        try
        {
            var user = new User { Email = "ef@test.com" };
            await _userServiceEf.Insert(user, false);
        }
        catch (Exception ex)
        {
            Console.WriteLine("ex1");
            Console.WriteLine(ex);
            throw;
        }
    }

    [Benchmark]
    public async Task Dapper_Insert()
    {
        var user = new User { Email = "ef@test.com" };
        await _userServiceDapper.Insert(user);
    }
    
    [Benchmark]
    public async Task Dapper_Contrib()
    {
        var user = new User { Email = "ef@test.com" };
        await _userServiceContrib.Insert(user);
    }
    
    [Benchmark]
    public async Task Dapper_Dommel()
    {
        var user = new User { Email = "ef@test.com" };
        await _userServiceDommel.Insert(user);
    }
    
    [Benchmark]
    public async Task Dapper_RepoDb()
    {
        var user = new User { Email = "ef@test.com" };
        await _userServiceRepoDb.Insert(user);
    }
    [Benchmark]
    public async Task Dapper_LinqToDb()
    {
        var user = new User { Email = "ef@test.com" };
        await _userServiceLinqToDb.Insert(user);
    }
    
    [Benchmark]
    public async Task Dapper_SimpleCrud()
    {
        var user = new User { Email = "ef@test.com" };
        await _userServiceSimpleCrud.Insert(user);
    }
    //
}