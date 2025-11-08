using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using Dapper;
using ef_base_repository;
using ef_dapper_models;
using RepoDb;

namespace repodb_implementation;



public partial class UserService_RepoDb 
{
    public async Task<int> Update(object user, string tableName = "Users")
    {
        var rawSql = GenericExpression.CreateUpdateQuery(user, tableName);

        await using var db = await GetOpenConnectionAsync();
        return await db.ExecuteAsync(rawSql, user);
    }
    
    public async Task<int> UpdateObject(object user, object key) 
    {
        await using var db = await GetOpenConnectionAsync();

        var rowsAffected = await db.UpdateAsync("Users", user, key);

        return rowsAffected;
    }
    public async Task<int> UpdateTypeSafe(UserToUpdate user)
    {
        await using var db = await GetOpenConnectionAsync();

        // Update returns number of rows affected
        return await db.UpdateAsync(user);
    }
}

