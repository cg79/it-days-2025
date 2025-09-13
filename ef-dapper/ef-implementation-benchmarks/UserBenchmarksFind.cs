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
public class UserBenchmarksFind: BaseBenchMark
{
    [GlobalSetup]
    public void Setup()
    {
        base.Setup();
    }

    [Benchmark]
    public async Task EfCore_FindWithLinq()
    {
        await _userServiceEf.FindWithLinq(
            el=>el.FirstName != null && 
            el.FirstName.Contains("Asia") && el.Age >= 18).ToListAsync();
    }


    [Benchmark]
    public async Task Dapper_Find_QueryFilter()
    {
        var filter = new QueryFilter
        {
            Conditions =
            {
                new QueryCondition { Field = "FirstName", Operator = SqlOperator.Contains, Value = "Join" },
                new QueryCondition { Field = "Age", Operator = SqlOperator.GreaterOrEqual, Value = 18 },
                // new QueryCondition { Field = "IsActive", Operator = SqlOperator.Equals, Value = true }
            }
        };
        await UserServiceDapper.Find(filter);
    }
    
    [Benchmark]
    public async Task Dapper_Find_SP()
    {
        await UserServiceDapper.FindWithSP();
    }
    
    [Benchmark]
    public async Task LinqToDb_Find_QueryFilter()
    {
        var filter = new QueryFilter
        {
            Conditions =
            {
                new QueryCondition { Field = "FirstName", Operator = SqlOperator.Contains, Value = "Join" },
                new QueryCondition { Field = "Age", Operator = SqlOperator.GreaterOrEqual, Value = 18 },
                // new QueryCondition { Field = "IsActive", Operator = SqlOperator.Equals, Value = true }
            }
        };
        await UserServiceLinqToDb.Find(filter);
    }
    
    [Benchmark]
    public async Task RepoDb_Find_QueryFilter()
    {
        var filter = new QueryFilter
        {
            Conditions =
            {
                new QueryCondition { Field = "FirstName", Operator = SqlOperator.Contains, Value = "Join" },
                new QueryCondition { Field = "Age", Operator = SqlOperator.GreaterOrEqual, Value = 18 },
                // new QueryCondition { Field = "IsActive", Operator = SqlOperator.Equals, Value = true }
            }
        };
        await UserServiceRepoDb.FindQueryFilter(filter);
    }
    
    [Benchmark]
    public async Task RepoDb_Find_Linq()
    {
        await UserServiceRepoDb.FindLinq(el=>el.FirstName.Contains("Asia") && el.Age >= 18);
    }
    
    
    //
}