using ef_base_repository;
using ef_dapper_models;
using Dapper;
using System.Data;
using System.Text.Json;
using ef_implementation;
using MySql.Data.MySqlClient;

namespace dapper_implementation;

public partial class UserService_Dapper
{
    string connectionString = "server=127.0.0.1;port=3306;database=MyAppDb;user=appuser;password=AppPass123;";
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

    public async Task<long> InsertRawWithSqlConnection(User employee)
    {
        const string sql = @"
                INSERT INTO Users (Email) 
                VALUES (@Email);
                SELECT LAST_INSERT_ID();";
        using (var connection = new MySqlConnection(connectionString))
        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@Email", employee.Email);
            connection.Open();
            object result = command.ExecuteScalar();
            return Convert.ToInt64(result);
        }
    }

    public async Task<long> InsertObject(object entity)
    {
        await using var db = await GetOpenConnectionAsync();
        var (sql, parameters)  = GenericExpression.CreateInsertQuery(entity, "Users");
        return await db.ExecuteScalarAsync<long>(sql, parameters);
    }

    public async Task<long> InsertWithSP(string email, string firstName)
    {
        await using var db = await GetOpenConnectionAsync();
        
        var newUser = await db.ExecuteScalarAsync<long>(
            "InsertUser1",
            new { p_FirstName = firstName, p_Email = email },
            commandType: CommandType.StoredProcedure
        );

        return newUser;
    }

   
}



