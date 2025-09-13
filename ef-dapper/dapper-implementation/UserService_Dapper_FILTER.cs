using ef_base_repository;
using ef_dapper_models;
using Dapper;
using System.Data;
using System.Text.Json;
using ef_implementation;

namespace dapper_implementation;

public partial class UserService_Dapper : BaseConnection
{
    public async Task<IEnumerable<User>> Filter(FilterGroup filter)
    {
        try
        {
            var (whereClause, parameters) = DapperFilterParser.Parse(filter);
            string sql = $"SELECT * FROM Users WHERE {whereClause}";
            
           await using var db = await GetOpenConnectionAsync();
            var filteredUsers = await db.QueryAsync<User>(sql, parameters);
            
            return filteredUsers;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}



