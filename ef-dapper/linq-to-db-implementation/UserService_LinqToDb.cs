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
    private IEFDataContext _dataContext;
    // private l2dbData.DataConnection _connection;
    // private IDataContext _linq2DbContext;
    public UserService_LinqToDb(IEFDataContext dataContext): base(dataContext)
    {
        this._dataContext = dataContext;
        // this._linq2DbContext = ((dataContext as DbContext)!).CreateLinqToDBContext();
        // this._connection = new l2dbData.DataConnection("MySql", dataContext.ConnectionString);
    }

    private IDataContext CreateLinqToDBContext()
    {
        return ((_dataContext as DbContext)!).CreateLinqToDBContext();
    }
   
    
}



