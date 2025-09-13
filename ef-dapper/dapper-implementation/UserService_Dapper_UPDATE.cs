using ef_base_repository;
using ef_dapper_models;
using Dapper;
using System.Data;
using System.Text.Json;
using ef_implementation;

namespace dapper_implementation;

public partial class UserService_Dapper
{
    public async Task<long> UpdateRawSql(User user)
    {
        const string sql = @"
            UPDATE Users
            SET FirstName = @FirstName,
                LastName = @LastName
            WHERE Id = @Id;";

        await using var db = _dataContext.GetDbConnection();
        return await db.ExecuteAsync(sql, user);
    }
    public async Task<int> UpdateObject(object user, string tableName = "Users")
    {
        var rawSql = GenericExpression.CreateUpdateQuery(user, tableName);

        await using var db = await GetOpenConnectionAsync();

        return await db.ExecuteAsync(rawSql, user);
    }
    
    public async Task<int> UpdateUsingTransaction(object user, string tableName, 
        IDbConnection db, IDbTransaction tx)
    {
        var rawSql = GenericExpression.CreateUpdateQuery(user, tableName);
        return await db.ExecuteAsync(rawSql, user, tx);
    }

   
}



