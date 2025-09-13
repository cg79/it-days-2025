using ef_base_repository;
using Microsoft.EntityFrameworkCore;

namespace ef_dapper;

public abstract class ServiceRegistration
{

    public static IServiceCollection RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // RegisterRepositories(services);
        AddDatabaseSettings(services, configuration);

        return services;
    }

    // private static IServiceCollection RegisterRepositories(IServiceCollection services)
    // {
    //     services.AddScoped<IUserRepository, UserRepository>();
    //     services.AddScoped<IUserCredentialsRepository, UserCredentialsRepository>();
    //     return services;
    // }

    private static IServiceCollection AddDatabaseSettings(IServiceCollection services, IConfiguration configuration)
    {
        // var connectionString = CreateConnectionString(configuration);
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        Console.WriteLine("CONNECTION_STRING_VALUE " + connectionString);

        services.AddScoped<IEFDataContext, DataContext>();
        services.AddDbContext<DataContext>(options =>
            options.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 3, 0)),
                mysqlOptions => mysqlOptions.EnableRetryOnFailure()
            ));

        return services;
    }

    private static string GetEnvironmentValue(string key, string defaultValue)
    {
        return Environment.GetEnvironmentVariable(key) ?? defaultValue;
    }

    private static string CreateConnectionString(IConfiguration configuration)
    {
        Console.WriteLine(configuration);
        var host = Environment.GetEnvironmentVariable("DBHOST") ?? "127.0.0.1";
        var port = GetEnvironmentValue("MYSQL_PORT", "3306");
        var password = GetEnvironmentValue("MYSQL_PASSWORD", "dev");
        var userid = GetEnvironmentValue("MYSQL_USER", "dev");
        var usersDataBase = GetEnvironmentValue("MYSQL_DATABASE", "test");

        var connectionString = $"server={host}; userid={userid};pwd={password};port={port};database={usersDataBase}";
        Console.WriteLine("connectionString-value" + connectionString);
        return connectionString;
    }

}