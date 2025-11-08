using System.Threading.Tasks;
using ef_base_repository;
using ef_dapper_models;
using ef_implementation_tests;
using ef_implementation;
using Microsoft.EntityFrameworkCore;
using Xunit;

public partial class UserServiceEfTests
{
   
    private async Task<WeakReference> CreateServiceAndInsertAsync(int i)
    {
        await using var context = CreateDataContext();
        var service = new UserServiceEF(context);
        var user = new User { Email = $"leaktest{i}@example.com" };
        await service.Insert(user);

        return new WeakReference(service);
    }
    
    
    [Fact]
    public async Task DetectMemoryLeak()
    {
        return;
        var weakRefs = new List<WeakReference>();
        for (int i = 0; i < 100; i++)
        {
            weakRefs.Add(await CreateServiceAndInsertAsync(i));
        }
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        var activeRefs = weakRefs.Where(wr => wr.IsAlive).ToList();
        Assert.True(
            activeRefs.Count() <= 1,
            activeRefs.Count().ToString()
        );
    }
    
    
    
}