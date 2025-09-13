using ef_dapper_models;

namespace ef_implementation;

public interface IUserServiceLinqToDb
{
   public Task<User> Insert(User user);
}