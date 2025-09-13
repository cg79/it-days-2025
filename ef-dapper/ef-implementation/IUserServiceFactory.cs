namespace ef_implementation;

public interface IUserServiceFactory
{
    IUserService GetService(UserServiceType key);
}