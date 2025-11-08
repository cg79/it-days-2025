using BenchmarkDotNet.Attributes;
using Bogus;
using dapper_contrib_implementation;
using dapper_implementation;
using Microsoft.EntityFrameworkCore;

using ef_base_repository;
using ef_dapper_models;
using ef_implementation_benchmarks;
using ef_implementation_tests;
using ef_implementation;
using linq_to_db_implementation;
using Org.BouncyCastle.Tls;
using repodb_implementation;

[MemoryDiagnoser]
public class UserBenchmarksFindMT : BaseBenchMark
{



    [GlobalSetup]
    public void Setup()
    {
        base.Setup();
    }

    Faker _faker;

    [Benchmark]
    public async Task EF_Find_QueryFilter_MT()
    {
        int age = _faker.Random.Int(2, 100);
        var filter = new QueryFilter
        {
            Conditions =
            {
                new QueryCondition { Field = "FirstName", Operator = SqlOperator.Contains, Value = "Join" },
                new QueryCondition { Field = "Age", Operator = SqlOperator.GreaterOrEqual, Value = age },
                // new QueryCondition { Field = "IsActive", Operator = SqlOperator.Equals, Value = true }
            }
        };
        await _userServiceEf.GetUserInvoicesMT(filter);
    }

    [Benchmark]
    public async Task EF_GetUsers_InvoicesAsyncMultipleQueries()
    {
        int age = _faker.Random.Int(2, 100);
        var filter = new QueryFilter
        {
            Conditions =
            {
                new QueryCondition { Field = "FirstName", Operator = SqlOperator.Contains, Value = "Join" },
                new QueryCondition { Field = "Age", Operator = SqlOperator.GreaterOrEqual, Value = age },
                // new QueryCondition { Field = "IsActive", Operator = SqlOperator.Equals, Value = true }
            }
        };
        await _userServiceEf.GetUsers_InvoicesAsyncMultipleQueries(filter);
    }

    [Benchmark]
    public async Task RepoDB_Find_QueryFilter_MT()
    {
        int age = _faker.Random.Int(2, 100);
        var filter = new QueryFilter
        {
            Conditions =
            {
                new QueryCondition { Field = "FirstName", Operator = SqlOperator.Contains, Value = "Join" },
                new QueryCondition { Field = "Age", Operator = SqlOperator.GreaterOrEqual, Value = age },
                // new QueryCondition { Field = "IsActive", Operator = SqlOperator.Equals, Value = true }
            }
        };
        await UserServiceRepoDb.GetUserInvoicesMT(filter);
    }


    //
}