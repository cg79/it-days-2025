using System;
using System.Linq;
using ef_base_repository;
using ef_implementation_tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TextToSqlDemo.Models;
using TextToSqlDemo.Services;

namespace TextToSqlDemo
{
    class Program : BaseTest
    {
        static DataContext CreateDataContext()
        {
             string connectionString = "server=127.0.0.1;port=3306;database=MyAppDb;user=appuser;password=AppPass123;";
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseMySql(connectionString, new MySqlServerVersion(new Version(8, 3, 0)))
                .LogTo(Console.WriteLine, LogLevel.Information) // logs SQL
                .EnableSensitiveDataLogging() 
                .Options;

            var context = new DataContext(options);

            // Ensure the schema exists
            context.Database.EnsureCreated();
            context.ConnectionString = connectionString;

            return context;
        }
        
        static async Task Main(string[] args)
        {
            // 1. Initialize DB and seed data
            
            const string API_KEY = "";


            // 2. Extract schema
            using var db = CreateDataContext();
            string schema = SchemaExtractor.GetDatabaseSchema(db);

            // 3. Setup SQL generator (AI)
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? API_KEY;
            var sqlGen = new SqlGenerator(apiKey);

            Console.WriteLine("=== Query Console ===");
            Console.WriteLine("Scrie o întrebare (ex: 'Afișează clienții cu vârsta > 30')");
            Console.WriteLine("Tastează 'exit' pentru a ieși.\n");

            var count = 0;

            while (true)
            {
                Console.Write("\n>> ");
                string? request = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(request)) continue;
                if (request.ToLower().Trim() == "exit") break;

                try
                {
                    string sql = "";
                    // 4. Get SQL from AI
                    if (count == 0)
                    {
                        count = 1;
                        sql = await sqlGen.InitOPenAiChatDb(request, schema);
                    }
                    else
                    {
                        sql = await sqlGen.GenerateSqlAsync(request);
                    }

                    Console.WriteLine($"\nSQL generat:\n{sql}");

                    // 5. Execute query
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}