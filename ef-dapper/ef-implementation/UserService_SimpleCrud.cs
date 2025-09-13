using ef_base_repository;
using ef_dapper_models;
using MySql.Data.MySqlClient;
using System.Data;
using Dapper;
using LinqToDB;
using LinqToDB.Data;
using simplecrud = Dapper.SimpleCRUD;

namespace ef_implementation;



public class UserService_SimpleCrud : IUserService
{
    // private  string _connectionString = "server=127.0.0.1;port=3306;database=MyAppDb;user=appuser;password=AppPass123;";
    // private IDbConnection Connection => new MySqlConnection(_connectionString);
    private IEFDataContext _dataContext;
    public UserService_SimpleCrud(IEFDataContext dataContext)
    {
        this._dataContext = dataContext;
        simplecrud.SetDialect(SimpleCRUD.Dialect.MySQL);
    }
    
    public async Task<User> Insert(User user)
    {
        try
        {
            var db =  _dataContext.GetDbConnection();
            if (db.State != ConnectionState.Open)
                await db.OpenAsync();
            
            //note: int?
            var id = await simplecrud.InsertAsync(db, user);
            await db.CloseAsync();
            user.Id = id ?? 0;
            return user;
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