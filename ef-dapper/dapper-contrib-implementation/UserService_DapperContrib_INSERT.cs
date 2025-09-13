using ef_base_repository;
using ef_dapper_models;
using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;

namespace dapper_contrib_implementation;



public partial class UserService_DapperContrib 
{
    public async Task<UserDapperContrib> Insert(UserDapperContrib user)
    {
        // await using var db = _dataContext.GetDbConnection();
        // if (db.State != ConnectionState.Open)
        //     await db.OpenAsync();
        // user.Id = await db.InsertAsync(user);
        // return user;
        
        await using var db = await GetOpenConnectionAsync();
        user.Id = await db.InsertAsync(user);
        return user;
    }

}
