using ef_base_repository;
using ef_dapper_models;
using System.Data;
using Dapper;
using Dommel;

namespace dapper_dommel_implementation;

public partial class UserService_Dommel
{
    public async Task<int> Update(object user, string tableName = "Users")
    {
        var rawSql = GenericExpression.CreateUpdateQuery(user, tableName);

        await using var db = await GetOpenConnectionAsync();
        return await db.ExecuteAsync(rawSql, user);
    }
}
