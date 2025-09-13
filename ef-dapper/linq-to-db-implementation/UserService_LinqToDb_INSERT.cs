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

public partial class UserService_LinqToDb : BaseConnection, IUserServiceLinqToDb
{
    
   
    
    public async Task<User> Insert(User user)
    {
        try
        {
            using (var db = CreateLinqToDBContext())
            {
                var id = await db.InsertWithInt64IdentityAsync(user);
                user.Id = Convert.ToInt64(id);
                return user;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}



