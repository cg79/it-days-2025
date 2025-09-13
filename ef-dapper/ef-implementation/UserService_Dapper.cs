using ef_base_repository;
using ef_dapper_models;
using Dapper;
using System.Data;
using System.Text.Json;

namespace ef_implementation;

public class UserService_Dapper : IUserService
{
    private IEFDataContext _dataContext;
    public UserService_Dapper(IEFDataContext dataContext)
    {
        this._dataContext = dataContext;
    }
    
    public async Task<User> Insert(User user)
    {
        try
        {
            const string sql = @"
                INSERT INTO Users (Email) 
                VALUES (@Email);
                SELECT LAST_INSERT_ID();";

            var db =  _dataContext.GetDbConnection();
            if (db.State != ConnectionState.Open)
                await db.OpenAsync();
            var id = await db.ExecuteScalarAsync<int>(sql, user);
            await db.CloseAsync();
            user.Id = id;
            return user;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    public async Task<User?> FindById(int id)
    {
        try
        {
            const string sql = $@"
            SELECT Id, Email
            FROM Users
            WHERE Id = @Id;";

            var db =  _dataContext.GetDbConnection();
            if (db.State != ConnectionState.Open)
                await db.OpenAsync();
            var user = await db.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
            return user;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<IEnumerable<User>> Find(QueryFilter filter)
    {
        try
        {
            var (whereClause, parameters) = filter.ToSqlWithParams();
            string sql = $"SELECT * FROM Users WHERE {whereClause}";
            
            var db =  _dataContext.GetDbConnection();
            if (db.State != ConnectionState.Open)
                await db.OpenAsync();
            var filteredUsers = await db.QueryAsync<User>(sql, parameters);
            await db.CloseAsync();
            
            return filteredUsers;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    
    public async Task<IEnumerable<User>> Filter(FilterGroup filter)
    {
        try
        {
            var (whereClause, parameters) = DapperFilterParser.Parse(filter);
            string sql = $"SELECT * FROM Users WHERE {whereClause}";
            
            var db =  _dataContext.GetDbConnection();
            if (db.State != ConnectionState.Open)
                await db.OpenAsync();
            var filteredUsers = await db.QueryAsync<User>(sql, parameters);
            await db.CloseAsync();
            
            return filteredUsers;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

   
}



// linq to db
// using (var db = new DataConnection())
// {
//     var users = db.GetTable<User>()
//         .Where(u => u.Email.Contains("test"))
//         .ToList();
// }