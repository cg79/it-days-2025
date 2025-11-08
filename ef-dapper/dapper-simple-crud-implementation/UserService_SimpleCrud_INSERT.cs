using ef_base_repository;
using ef_dapper_models;
using MySql.Data.MySqlClient;
using System.Data;
using Dapper;

namespace dapper_simple_crud_implementation;

public partial class UserService_SimpleCrud 
{
    public async Task<User> Insert(User user, IDbTransaction transaction = null)
    {
        await using var db = await GetOpenConnectionAsync();

        //note: int?
        var id = await db.InsertAsync(user, transaction);
        user.Id = id ?? 0;
        return user;
    }
    
}
