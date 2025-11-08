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
public class UserBenchmarksUpdateByIdEF: BaseBenchMark
{

    [GlobalSetup]
    public void Setup()
    {
        base.Setup();
    }

    [Benchmark]
    public async Task EF_UpdateByIdUsingEntityAndRawSql()
    {
        var fn = Guid.NewGuid().ToString();
        var user = new
        {
            Id = 7,
            FirstName = fn,
            Email = "test@example.com"
        };
        var updated = await _userServiceEf.UpdateByIdUsingEntityAndRawSql(user, "Users");
        if (updated == 0)
        {
            throw new Exception("Update failed");
        }
    }

    [Benchmark]
    public async Task EF_UpdateByIdAsyncTRACKED()
    {
        var fn = Guid.NewGuid().ToString();
        var user = new User { Email = "ef@test.com" };
        var updated = await _userServiceEf.UpdateByIdAsyncTRACKED(6,
            u => { u.FirstName = fn; },
            new CancellationToken());
        if (updated == 0)
        {
            throw new Exception("Update failed");
        }
    }

    [Benchmark]
    public async Task EF_UpdateByIdAsyncUNTRACKED()
    {
        var fn = Guid.NewGuid().ToString();

        // Act
        var updated = await _userServiceEf.UpdateByIdAsyncUNTRACKED(
            7,
            u=> { u.FirstName = fn; },
            new CancellationToken());
        if (updated == 0)
        {
            throw new Exception("Update failed");
        }
    }

    [Benchmark]
    public async Task EF_UpdateById_CreateRawSQL_3()
    {
        // Arrange
        var fn = Guid.NewGuid().ToString();

        var user = new 
        {
            Id = 7,
            FirstName = fn,
            Email = "test@example.com"
        };

          var updated =  await _userServiceEf.UpdateByIdUsingEntityAndRawSql(
                user,
                "Users",
                new CancellationToken());
          if (updated == 0)
          {
              throw new Exception("Update failed");
          }
    }
}