using ef_base_repository;
using ef_dapper_models;
using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Tools;

using l2db = LinqToDB;
using l2dbData = LinqToDB.Data;

namespace ef_implementation;

public class UserService_LinqToDb : IUserServiceLinqToDb
{
    private IEFDataContext _dataContext;
    private l2dbData.DataConnection _connection;
    public UserService_LinqToDb(IEFDataContext dataContext)
    {
        this._dataContext = dataContext;
        this._connection = new l2dbData.DataConnection("MySql", dataContext.ConnectionString);
    }
   
    
    public async Task<User> Insert(User user)
    {
        try
        {
            if (_connection.Connection.State != ConnectionState.Open)
                await _connection.Connection.OpenAsync();
            var id = await _connection.InsertWithInt32IdentityAsync(user, "Users");
            await _connection.Connection.CloseAsync();
            user.Id = id;
            return user;
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    public async Task<IEnumerable<User>> Find(QueryFilter filter)
    {
        try
        {

            var predicate = filter.ToExpression<User>();
            
            if (_connection.Connection.State != ConnectionState.Open)
                await _connection.Connection.OpenAsync();
            var tbl = _connection.GetTable<UserLinqToDb>();
            var filteredUsers = tbl    
                .Where(predicate)
                .ToList();
            await _connection.Connection.CloseAsync();
            
            return filteredUsers;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    public async Task<IEnumerable<IUser>> Filter(FilterGroup filter)
    {
        try
        {
            
            var predicate = LinqFilterBuilder<UserLinqToDb>.Build(filter);
            
            if (_connection.Connection.State != ConnectionState.Open)
                await _connection.Connection.OpenAsync();
            var tbl = _connection.GetTable<UserLinqToDb>();
            var filteredUsers = tbl    
                .Where(predicate)
                .ToList();
            await _connection.Connection.CloseAsync();
            
            return filteredUsers;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    public async Task<IEnumerable<UserLinqToDb_CustomFields>> FilterWithCustomFileds(FilterGroup filter)
    {
        try
        {
            
            var predicate = LinqFilterBuilder<UserLinqToDb_CustomFields>.Build(filter);
            
            if (_connection.Connection.State != ConnectionState.Open)
                await _connection.Connection.OpenAsync();
            var tbl = _connection.GetTable<UserLinqToDb_CustomFields>();
            var filteredUsers = tbl    
                .Where(predicate)
                .ToList();
            await _connection.Connection.CloseAsync();
            
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