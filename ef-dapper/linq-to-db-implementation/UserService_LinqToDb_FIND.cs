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
   
    public async Task<IEnumerable<User>> Find(QueryFilter filter)
    {

        var predicate = filter.ToExpression<User>();

        using (var db = CreateLinqToDBContext())
        {
            var tbl = db.GetTable<UserLinqToDb>();
            var filteredUsers = tbl
                .Where(predicate)
                .ToList();

            return filteredUsers;
        }
    }
    public async Task<IEnumerable<IUser>> Filter(FilterGroup filter)
    {
        try
        {
            
            var predicate = LinqFilterBuilder<UserLinqToDb>.Build(filter);
            
            using (var db = CreateLinqToDBContext())
            {
                var tbl = db.GetTable<UserLinqToDb>();
                var filteredUsers = tbl    
                    .Where(predicate)
                    .ToList();
            
                return filteredUsers;
            }

            
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<UserLinqToDb_CustomFields>> FilterWithCustomFileds(FilterGroup filter)
    {

        var predicate = LinqFilterBuilder<UserLinqToDb_CustomFields>.Build(filter);

        // if (_connection.Connection.State != ConnectionState.Open)
        //     await _connection.Connection.OpenAsync();

        using (var db = CreateLinqToDBContext())
        {
            var tbl = db.GetTable<UserLinqToDb_CustomFields>();
            var filteredUsers = tbl
                .Where(predicate)
                .ToList();

            return filteredUsers;
        }

        
    }
}



