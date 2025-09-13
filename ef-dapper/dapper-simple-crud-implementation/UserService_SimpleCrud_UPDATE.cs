using ef_base_repository;
using ef_dapper_models;
using MySql.Data.MySqlClient;
using System.Data;
using Dapper;

namespace dapper_simple_crud_implementation;

public partial class UserService_SimpleCrud 
{
    public async Task<int> UpdateObject(object user)
    {
        var rawSql = GenericExpression.CreateUpdateQuery(user, "Users");

        await using var db = await GetOpenConnectionAsync();

        return await db.ExecuteAsync(rawSql, user);
    }
    
    public async Task<int> UpdateTypeSafe(UserToUpdate user)
    {
        await using var db = await GetOpenConnectionAsync();

        // Update returns number of rows affected
        return await db.UpdateAsync(user);
    }

    public async Task<int> Update(object user, string tableName = "Users")
    {
        var rawSql = GenericExpression.CreateUpdateQuery(user, tableName);

        await using var db = await GetOpenConnectionAsync();
        return await db.ExecuteAsync(rawSql, user);
    }
}
