﻿
using ef_base_repository;
using ef_dapper_CustomDataSeed;
using Microsoft.EntityFrameworkCore;

namespace ef_dapper
{
    public abstract class Program
    {
        public static async Task Main(string[] args)
        {
            var app = CreateHostBuilder(args).Build();
            using (var scope = app.Services.CreateScope())
            {
                var seed = false;
                if (seed)
                {
                    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
                    db.Database.EnsureCreated();

                    // await DataSeeder.SeedAsync(db, 10_000);
                    // db.Database.Migrate();

                    CustomDataSeed customDataSeed = new CustomDataSeed();
                    
                    string jsonConfig = File.ReadAllText("CustomDataSeed/seedConfig.json");
                    await customDataSeed.SeedDatabaseAsync(db, jsonConfig);
                }
            } 
            app.Run();
        }

        private static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
                
    }
}