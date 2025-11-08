using ef_base_repository;
using ef_dapper_models;
using System.Data;
using Dapper;
using ef_implementation;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using LinqToDB.Tools;
using Microsoft.EntityFrameworkCore;

namespace linq_to_db_implementation;

public partial class UserService_LinqToDb 
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

        // using (var db = CreateLinqToDBContext())
        // {
        //     return await db.Exe(rawSql, user);
        //     // await db.ExecuteAsync(rawSql, user);
        // }

        await using var db = await GetOpenConnectionAsync();
        var response = await db.ExecuteAsync(rawSql, user);
        return response;
    }
}



