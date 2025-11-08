using BenchmarkDotNet.Attributes;
using Bogus;
using Microsoft.EntityFrameworkCore;

using ef_base_repository;
using ef_dapper_models;
using ef_implementation_benchmarks;

[MemoryDiagnoser] 
public class UserBenchmarksFind_products_1M: BaseBenchMark
{
    public Faker<Products> faker { get; set; }
    [GlobalSetup]
    public void Setup()
    {
        base.Setup();
         this.faker = new Faker<Products>().RuleFor(p => p.Name, f => f.Commerce.ProductName()) // random product name
            .RuleFor(p => p.Price, f => f.Random.Decimal(5, 5000));
    }
    
    [Benchmark]
    public async Task RepoDb_Find_QueryFilter()
    {
        try
        {
            var prod = faker.Generate(1)[0];
            var filter = new QueryFilter
            {
                Conditions =
                {
                    new QueryCondition { Field = "Name", Operator = SqlOperator.Contains, Value = prod.Name },
                    new QueryCondition { Field = "Price", Operator = SqlOperator.GreaterOrEqual, Value = prod.Price },
                }
            };
            await ProductService_RepoDb.FindQueryFilter(filter);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    
    [Benchmark]
    public async Task EfCore_FindProductsWithLinq()
    {
        var dbContext = GetMySqlDbContext();
        var prodRepo = new GenericRepository<Products>(dbContext);
        
        var prod = faker.Generate(1)[0];
        await prodRepo.FindLinq(
            el=>
            el.Name.Contains(prod.Name) && el.Price >= prod.Price).ToListAsync();
    }
    
    [Benchmark]
    public async Task Dapper_Find_QueryFilter()
    {
        var prod = faker.Generate(1)[0];
        var filter = new QueryFilter
        {
            Conditions =
            {
                new QueryCondition { Field = "Name", Operator = SqlOperator.Contains, Value = prod.Name },
                new QueryCondition { Field = "Price", Operator = SqlOperator.GreaterOrEqual, Value = prod.Price },
            }
        };
        await ProductServiceDapper.Find(filter);
    }
    
    [Benchmark]
    public async Task Dapper_Find_SP()
    {
        var prod = faker.Generate(1)[0];
        await ProductServiceDapper.FindProductsWithSP(prod.Name, prod.Price);
    }
    
    [Benchmark]
    public async Task LinqToDb_Find_QueryFilter()
    {
        var prod = faker.Generate(1)[0];
        var filter = new QueryFilter
        {
            Conditions =
            {
                new QueryCondition { Field = "Name", Operator = SqlOperator.Contains, Value = prod.Name },
                new QueryCondition { Field = "Price", Operator = SqlOperator.GreaterOrEqual, Value = prod.Price },
            }
        };
        await ProductServiceDapper.Find(filter);
    }
    
    
    [Benchmark]
    public async Task RepoDb_Find_Linq()
    {
        var prod = faker.Generate(1)[0];
        await ProductService_RepoDb.FindLinq(el=>el.Name.Contains(prod.Name) && el.Price >=prod.Price);
    }
    
    [Benchmark]
    public async Task Dommel_Find_QueryFilter()
    {
        var prod = faker.Generate(1)[0];
        var filter = new QueryFilter
        {
            Conditions =
            {
                new QueryCondition { Field = "Name", Operator = SqlOperator.Contains, Value = prod.Name },
                new QueryCondition { Field = "Price", Operator = SqlOperator.GreaterOrEqual, Value = prod.Price },
            }
        };
        await ProductService_Dommel.FindQueryFilterToPredicate(filter);
    }
    
    [Benchmark]
    public async Task Dommel_Find_Linq()
    {
        var prod = faker.Generate(1)[0];
        await ProductService_Dommel.FindLinq(el=>el.Name.Contains(prod.Name) && el.Price >=prod.Price);
    }
    
}