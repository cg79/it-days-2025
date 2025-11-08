using ef_base_repository;
using ef_dapper_models;
using System.Data;
using Dapper;
using Dommel;

namespace dapper_dommel_implementation;

public partial class UserService_Dommel
{

    public async Task<User> Insert(User user)
    {
        await using var db = await GetOpenConnectionAsync();

        var id = await db.InsertAsync(user);
        user.Id = Convert.ToInt64(id);
        return user;
    }
}
