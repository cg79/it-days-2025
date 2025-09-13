using ef_base_repository;
using ef_dapper_models;
using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;

namespace dapper_contrib_implementation;



public partial class UserService_DapperContrib 
{
    public async Task<int> Update(object user, string tableName = "Users")
    {
        var rawSql = GenericExpression.CreateUpdateQuery(user, tableName);

        await using var db = await GetOpenConnectionAsync();
        return await db.ExecuteAsync(rawSql, user);
    }
}
