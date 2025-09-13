using ef_dapper_models;

namespace ef_implementation;

public interface IUserService
{
   public Task<User> Insert(User user);
}