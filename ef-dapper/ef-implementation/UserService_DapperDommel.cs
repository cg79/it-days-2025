using ef_base_repository;
using ef_dapper_models;
using System.Data;
using dommel = Dommel.DommelMapper;               // optional if you want to alias Dommel mapping

namespace ef_implementation;



public class UserService_Dommel
{
    private IEFDataContext _dataContext;
    public UserService_Dommel(IEFDataContext dataContext)
    {
        this._dataContext = dataContext;
        // dapperContrib.SetDialect(SimpleCRUD.Dialect.MySQL);
    }
    
    public async Task<User> Insert(User user)
    {
        try
        {
            var db =  _dataContext.GetDbConnection();
            if (db.State != ConnectionState.Open)
                await db.OpenAsync();


            var id = await dommel.InsertAsync(db,user);
            await db.CloseAsync();
            
            user.Id = Convert.ToInt64(id);
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