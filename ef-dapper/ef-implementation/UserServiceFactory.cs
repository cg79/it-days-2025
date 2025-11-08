using Microsoft.Extensions.DependencyInjection;

namespace ef_implementation;

public class UserServiceFactory 
{
    private readonly IServiceProvider _provider;
    public UserServiceFactory(IServiceProvider provider)
    {
        _provider = provider;
    }

    // public IUserService GetService(UserServiceType key)
    // {
    //     return key switch
    //     {
    //         // UserServiceType.EF => _provider.GetRequiredService<UserServiceEF>(),
    //         // UserServiceType.Dapper => _provider.GetRequiredService<UserService_Dapper>(),
    //         // UserServiceType.LinqToDb => _provider.GetRequiredService<UserService_LinqToDb>(),
    //         // UserServiceType.SimpleCrud => _provider.GetRequiredService<UserService_SimpleCrud>(),
    //         // "service2" => _provider.GetRequiredService<UserService_Dapper>(),
    //         _ => throw new ArgumentException("Invalid key")
    //     };
    // }
}

