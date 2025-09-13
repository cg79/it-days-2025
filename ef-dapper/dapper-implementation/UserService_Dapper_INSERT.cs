using ef_base_repository;
using ef_dapper_models;
using Dapper;
using System.Data;
using System.Text.Json;
using ef_implementation;

namespace dapper_implementation;

public partial class UserService_Dapper
{
    public async Task<User> InsertRawSql(User user)
    {
        const string sql = @"
                INSERT INTO Users (Email) 
                VALUES (@Email);
                SELECT LAST_INSERT_ID();";

        await using var db = await GetOpenConnectionAsync();
        user.Id = await db.ExecuteScalarAsync<long>(sql, user);
        return user;
    }
    
    public async Task<long> InsertObject(object entity)
    {
        await using var db = await GetOpenConnectionAsync();
        var (sql, parameters)  = GenericExpression.CreateInsertQuery(entity, "Users");
        return await db.ExecuteScalarAsync<long>(sql, parameters);
    }

   
}



