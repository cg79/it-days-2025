using BenchmarkDotNet.Attributes;
using dapper_contrib_implementation;
using dapper_implementation;
using Microsoft.EntityFrameworkCore;

using ef_base_repository;
using ef_dapper_models;
using ef_implementation_benchmarks;
using ef_implementation_tests;
using ef_implementation;
using Efcom.Base.Repository.Request;
using linq_to_db_implementation;
using repodb_implementation;

[MemoryDiagnoser] 
public class UserBenchmarksPagination: BaseBenchMark
{
    [GlobalSetup]
    public void Setup()
    {
        base.Setup();
    }

    [Benchmark]
    public async Task EfCore_Pagination()
    {
        var paginationRequest = new PaginationRequest()
        {
            PageNo = 1,
            PageSize = 7,
            SortCriteria = new Dictionary<string, bool>
            {
                { "PhoneNumber", false },
                { "FirstName", true }
            },
            FilterGroup = new FilterGroup
            {
                Logic = "and",
                Filters = new List<object>
                {
                    new FilterCondition { Field = "Id", Operator = FilterOperator.Gt, Value = 10 },
                    new FilterGroup
                    {
                        Logic = "or",
                        Filters = new List<object>
                        {
                            new FilterCondition { Field = "Email", Operator = FilterOperator.Contains, Value = "Verona" },
                            new FilterCondition { Field = "Email", Operator = FilterOperator.Contains, Value = "Price" }
                        }
                    }
                }
            }
        };
        await _userServiceEf.PaginatedUsers(
            paginationRequest);
    }


    [Benchmark]
    public async Task DommedlPagination()
    {
        var paginationRequest = new PaginationRequest()
        {
            PageNo = 1,
            PageSize = 7,
            SortCriteria = new Dictionary<string, bool>
            {
                { "PhoneNumber", false },
                { "FirstName", true }
            },
            FilterGroup = new FilterGroup
            {
                Logic = "and",
                Filters = new List<object>
                {
                    new FilterCondition { Field = "Id", Operator = FilterOperator.Gt, Value = 10 },
                    new FilterGroup
                    {
                        Logic = "or",
                        Filters = new List<object>
                        {
                            new FilterCondition { Field = "Email", Operator = FilterOperator.Contains, Value = "Verona" },
                            new FilterCondition { Field = "Email", Operator = FilterOperator.Contains, Value = "Price" }
                        }
                    }
                }
            }
        };
        await UserServiceDommel.Pagination(paginationRequest);
    }
}