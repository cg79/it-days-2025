-- docker run --name mysql-db -e MYSQL_ROOT_PASSWORD=RootPass123 -e MYSQL_DATABASE=MyAppDb -p 3306:3306 -d mysql:8.3

# docker my-sql image
```
docker run --name mysql-db \
-e MYSQL_ROOT_PASSWORD=RootPass123 \
-e MYSQL_DATABASE=MyAppDb \
-e MYSQL_USER=appuser \
-e MYSQL_PASSWORD=AppPass123 \
-p 3306:3306 -d mysql:8.3
```

# postgress
```aiignore
docker run -d \
  --name pgvector-db \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=embeddingsdb \
  -p 5433:5432 \
  ankane/pgvector
```

# redis
```
docker run -d --name my-redis -p 6379:6379 redis
```


https://www.youtube.com/watch?v=W5BxH9CdcsY&t=626s

```aiignore sql history
SET GLOBAL log_output = 'TABLE'; 
SET GLOBAL general_log = 'ON';
SELECT event_time, argument FROM mysql.general_log  ORDER BY event_time DESC LIMIT 50;

```

# migration
```C#
public static void Main(string[] args)
{
    var app = CreateHostBuilder(args).Build();
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<DataContext>();
        db.Database.EnsureCreated();
        app.Run();
    } 
    await DataSeeder.SeedAsync(db, 10_000);
    app.Run();
}
```

# benchmarks
 - run from the benchmarks directory
```aiignore
dotnet run -c Release
or
dotnet run -c Release -- --diagnosers *
```

# functionalities 
    - Insert
        - Insert (use only few columns)
    - FindById
    - Filter (find)
        - Filter (Return only specific columns)
    - sub - queries (CTE)
    - joins
    - mapping table to structured data

BenchmarkDotNet are built to measure performance, not to detect memory leaks.

Tools to detect memory leaks in .NET
    - BenchmarkDotNet + Diagnosers
        - Great for spotting excessive allocations, but not leaks.
    - dotMemory Unit (JetBrains)
    - Profilers - etBrains dotMemory, Redgate ANTS Memory Profiler, or Visual Studio Diagnostic Tools
    - PerfView
        - Free, powerful, from Microsoft.
    - GC + WeakReference hack (DIY test)
        
```
private async Task<WeakReference> CreateServiceAndInsertAsync(int i)
    {
        await using var context = CreateDataContext();
        var service = new UserService_Dapper(context);
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

        // Force GC
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();


        var activeRefs = weakRefs.Where(wr => wr.IsAlive).ToList();
        Console.WriteLine(activeRefs.Count());
        // Assert all weak refs have been collected
        Assert.True(
            activeRefs.Count() <= 1,
            activeRefs.Count().ToString()
        );
    }
        ```

1️⃣ Dapper.SimpleCRUD

Purpose: Automatic CRUD operations without writing SQL.

Pros: Auto-generates INSERT, UPDATE, DELETE, SELECT; works with attributes.

Cons: Limited customization, mostly for simple cases.

NuGet: Dapper.SimpleCRUD

Example: db.InsertAsync(user);

2️⃣ Dapper.Contrib

Purpose: Lightweight helper for basic CRUD.

Pros: Part of official Dapper repository; supports [Key] attributes.

Cons: Less flexible than raw SQL.

NuGet: Dapper.Contrib

Example: db.Insert(user) / db.Get<User>(id)

3️⃣ Dommel

Purpose: Adds LINQ-like functionality on top of Dapper.

Pros: db.Select<T>(x => x.Email == "abc") works like LINQ.

Cons: Limited to simple queries, not full LINQ provider.

NuGet: Dommel

Example:

var user = db.Select<User>(x => x.Email == "abc").FirstOrDefault();

4️⃣ Dapper.Rainbow

Purpose: Lightweight active-record style ORM on top of Dapper.

Pros: Very simple to use; no explicit SQL for basic CRUD.

Cons: Not as widely used or maintained.

NuGet: Dapper.Rainbow

Example:

var userTable = new Table<User>(db);
userTable.Insert(user);

5️⃣ Linq2Db + Dapper integration

Purpose: Use LINQ queries but still get Dapper speed for execution.

Pros: Full LINQ support, works well with strongly-typed models.

Cons: Adds another layer; learning curve.

NuGet: linq2db, optionally linq2db.Dapper

Example: db.GetTable<User>().Where(u => u.Email.Contains("test")).ToList();

6️⃣ RepoDb

Purpose: High-performance ORM for SQL Server / MySQL / PostgreSQL.

Pros: Very fast, supports batch operations and async CRUD.

Cons: Less community adoption than Dapper itself.

NuGet: RepoDb

Example: db.InsertAsync(user);

⚡ Summary

For CRUD helpers: Dapper.SimpleCRUD, Dapper.Contrib, Dapper.Rainbow

For LINQ-like queries: Dommel, linq2db

For high-performance ORM: RepoDb

SELECT u.*
  FROM `Users` AS `u`
  LEFT JOIN (
      SELECT `i`.`InvoiceDate`, `i`.`UserId`
      FROM `Invoices` AS `i`
      WHERE `i`.`Status` = 'Paid'
  ) AS `i0` ON `u`.`Id` = `i0`.`UserId`
  WHERE `u`.`Id` = 6


practices
    - start with unit test
    - EF should return IQuerable

- use CancellationToken
- when updating, do not update all fields
- when a connection should be closed
    - how i can reuse the connection