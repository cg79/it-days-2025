using ef_base_repository;
using ef_dapper_models;
using LinqToDB.EntityFrameworkCore;

namespace ef_dapper;

using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class DataSeederProducts
{
    public static async Task SeedProductsAsync(DataContext db, int count)
    {
        // clear old data
        await db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Products");

        var faker = new Faker<Products>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price(5, 500)));

        var batchCount = 10000;
        var steps = Math.Ceiling((double)count / batchCount) + 1;
        var index = 0;
        while (index < steps)
        {
            await AddProductsToDb(db, faker);
            db.ChangeTracker.Clear();
            index = index+1;
            await Task.Delay(1000);
        }
    }

    private static async Task AddProductsToDb(DataContext db, Faker<Products> fakerProducts, int count = 10_000)
    {
        var products = fakerProducts.Generate(count);

        await db.BulkCopyAsync(products);
        // await db.Products.AddRangeAsync(products);
        await db.SaveChangesAsync();
    }
}
