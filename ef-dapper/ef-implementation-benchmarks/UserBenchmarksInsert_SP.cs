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
public class UserBenchmarksInsertSP: BaseBenchMark
{

    [GlobalSetup]
    public void Setup()
    {
        base.Setup();
    }

   
    [Benchmark]
    public async Task Dapper_Insert_SP()
    {
        await UserServiceDapper.InsertWithSP("ef-base-repository@as.com", "amd");
    }
}