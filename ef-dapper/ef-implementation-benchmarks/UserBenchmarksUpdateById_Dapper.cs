using BenchmarkDotNet.Attributes;
using ef_dapper_models;
using ef_implementation_benchmarks;

[MemoryDiagnoser] 
public class UserBenchmarksUpdateByIdDapper: BaseBenchMark
{

    [GlobalSetup]
    public void Setup()
    {
        base.Setup();
    }
    
    [Benchmark]
    public async Task Dapper_UpdateByIdUsingEntityAndRawSql()
    {
        var fn = Guid.NewGuid().ToString();
        var user = new User
        {
            Id = 7,
            FirstName = fn,
            Email = "test@example.com"
        };
        var updated = await UserServiceDapper.UpdateRawSql(user);
        if (updated == 0)
        {
            throw new Exception("Update failed");
        }
    }
    
    [Benchmark]
    public async Task Dapper_Update()
    {
        var fn = Guid.NewGuid().ToString();
        var user = new User
        {
            Id = 7,
            FirstName = fn,
            Email = "test@example.com"
        };
        var updated = await UserServiceDapper.UpdateObject(user);
        if (updated == 0)
        {
            throw new Exception("Update failed");
        }
    }
    
    [Benchmark]
    public async Task Dapper_Contrib_Update()
    {
        var fn = Guid.NewGuid().ToString();
        var user = new User
        {
            Id = 7,
            FirstName = fn,
            Email = "test@example.com"
        };
        var updated = await UserServiceContrib.Update(user);
        if (updated == 0)
        {
            throw new Exception("Update failed");
        }
    }
    
    [Benchmark]
    public async Task RepoDB_Update()
    {
        var fn = Guid.NewGuid().ToString();
        var user = new User
        {
            Id = 7,
            FirstName = fn,
            Email = "test@example.com"
        };
        var updated = await UserServiceRepoDb.Update(user);
        if (updated == 0)
        {
            throw new Exception("Update failed");
        }
    }
    
    [Benchmark]
    public async Task Dommel_Update()
    {
        var fn = Guid.NewGuid().ToString();
        var user = new User
        {
            Id = 7,
            FirstName = fn,
            Email = "test@example.com"
        };
        var updated = await UserServiceDommel.Update(user);
        if (updated == 0)
        {
            throw new Exception("Update failed");
        }
    }
    
    [Benchmark]
    public async Task SimpleCrud_Update()
    {
        var fn = Guid.NewGuid().ToString();
        var user = new User
        {
            Id = 7,
            FirstName = fn,
            Email = "test@example.com"
        };
        var updated = await UserServiceSimpleCrud.Update(user);
        if (updated == 0)
        {
            throw new Exception("Update failed");
        }
    }
    
    [Benchmark]
    public async Task Linq2DB_Update()
    {
        var fn = Guid.NewGuid().ToString();
        var user = new User
        {
            Id = 7,
            FirstName = fn,
            Email = "test@example.com"
        };
        var updated = await UserServiceLinqToDb.Update(user);
        if (updated == 0)
        {
            throw new Exception("Update failed");
        }
    }
    
    
    
    
    

    
}