using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using ef_base_repository;
using ef_dapper_models;
using RepoDb;

namespace repodb_implementation;

public partial class UserService_RepoDb
{
    
    
    public async Task<User> Insert(User user, IDbTransaction? transaction = null)
    {
        await using var db = await GetOpenConnectionAsync();

        var id = await db.InsertAsync(user, transaction: transaction);
        user.Id = Convert.ToInt64(id);
        return user;
    }

    
    
    


}

