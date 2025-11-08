using ef_base_repository;
using ef_dapper_models;
using Dapper;
using System.Data;
using System.Text.Json;
using ef_implementation;

namespace dapper_implementation;

public partial class UserService_Dapper : BaseConnection
{
    public async Task<User?> FindById(long id)
    {
        try
        {
            const string sql = $@"
            SELECT Id, Email
            FROM Users
            WHERE Id = @Id;";

           await using var db = await GetOpenConnectionAsync();
            var user = await db.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
            return user;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<IEnumerable<User>> Find(QueryFilter filter, string fields = "*")
    {
        var (whereClause, parameters) = filter.ToSqlWithParams();
        string sql = $"SELECT {fields} FROM Users WHERE {whereClause}";

        await using var db = await GetOpenConnectionAsync();
        var filteredUsers = await db.QueryAsync<User>(sql, parameters);

        return filteredUsers;
    }

    public async Task<IEnumerable<User>> FindWithSP()
    {
        await using var db = await GetOpenConnectionAsync();
        
        var filteredUsers = await db.QueryAsync<User>(
            "FindUsersByNameAndAge",
            new { p_firstName = "ana", p_minAge = 18 },
            commandType: CommandType.StoredProcedure
        );

        return filteredUsers;
    }
    
    
}



