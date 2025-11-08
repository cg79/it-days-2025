using BenchmarkDotNet.Attributes;
using dapper_contrib_implementation;
using dapper_implementation;
using Microsoft.EntityFrameworkCore;

using ef_base_repository;
using ef_dapper_models;
using ef_implementation_benchmarks;
using ef_implementation_tests;
using ef_implementation;
using linq_to_db_implementation;
using repodb_implementation;

[MemoryDiagnoser] 
public class UserBenchmarksInsert: BaseBenchMark
{

    [GlobalSetup]
    public void Setup()
    {
        base.Setup();
    }

    [Benchmark]
    public async Task EfCore_Insert()
    {
            var user = new User { Email = "ef@test.com" };
            await _userServiceEf.Insert(user);
    }
    
    [Benchmark]
    public async Task EfCore_InsertNOTRACKING()
    {
            var user = new User { Email = "ef@test.com" };
            await _userServiceEf.InsertNoTracking(user);
    }
    
    [Benchmark]
    public async Task ADO_NET_Insert_Raw_SQL()
    {
        var user = new User { Email = "ef@test.com" };
        await UserServiceDapper.InsertRawWithSqlConnection(user);
    }

    [Benchmark]
    public async Task Dapper_Insert_Raw_SQL()
    {
        var user = new User { Email = "ef@test.com" };
        await UserServiceDapper.InsertRawSql(user);
    }
    
    [Benchmark]
    public async Task Dapper_Insert_Object()
    {
        var user = new  { Email = "ef@test.com" };
        await UserServiceDapper.InsertObject(user);
    }
    
    [Benchmark]
    public async Task Dapper_Contrib()
    {
        var user = new UserDapperContrib() { Email = "ef@test.com" };
        await UserServiceContrib.Insert(user);
    }
    
    [Benchmark]
    public async Task Dapper_Dommel()
    {
        var user = new User { Email = "ef@test.com" };
        await UserServiceDommel.Insert(user);
    }
    
    [Benchmark]
    public async Task Dapper_RepoDb()
    {
        var user = new User { Email = "ef@test.com" };
        await UserServiceRepoDb.Insert(user);
    }
    [Benchmark]
    public async Task Dapper_LinqToDb()
    {
        var user = new User { Email = "ef@test.com" };
        await UserServiceLinqToDb.Insert(user);
    }
    
    [Benchmark]
    public async Task Dapper_SimpleCrud()
    {
        var user = new User { Email = "ef@test.com" };
        await UserServiceSimpleCrud.Insert(user);
    }
    //
}