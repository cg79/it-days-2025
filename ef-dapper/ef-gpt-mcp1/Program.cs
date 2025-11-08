using ef_gpt_mcp1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class Program
{
    static async Task Main(string[] args)
    {
        // var configuration = new ConfigurationBuilder()
        //     .SetBasePath(AppContext.BaseDirectory)
        //     //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //     .Build();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        /*
        var connectionString = $"Data Source={configuration["Database:Path"]}";

        var schemaService = new SqliteSchemaService(connectionString);

        Console.WriteLine("=== Tables ===");
        var tables = schemaService.GetTables().Take(2);
        var tablesYaml = tables.ToYaml();
        Console.WriteLine(tablesYaml);

        Console.WriteLine("=== Specific Table: Loans ===");
        var table = schemaService.GetTable("Loans");
        var tableYaml = table?.ToYaml() ?? "";
        Console.WriteLine(tableYaml);

        Console.WriteLine("=== Views ===");
        var views = schemaService.GetViews().Take(2);
        var viewsYaml = views.ToYaml();
        Console.WriteLine(viewsYaml);

        Console.WriteLine("=== Foreign Key Relations ===");
        var relations = schemaService.GetForeignKeyRelations().Take(2);
        var relationsYaml = relations.ToYaml();
        Console.WriteLine(relationsYaml);
        */

        var builder = Host.CreateEmptyApplicationBuilder(settings: null);

        CraftDbMcpServer.Configure(configuration);

        builder.Services
            .AddMcpServer()
            .WithStdioServerTransport()
            .WithToolsFromAssembly();

        await builder.Build().RunAsync();

    }

}