using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using ef_base_repository;
using ef_dapper_models;
using RepoDb;

namespace repodb_implementation;

public partial class UserService_RepoDb: BaseConnection
{
    private IEFDataContext _dataContext;
    // private DbConnection _db;
    public UserService_RepoDb(IEFDataContext dataContext): base(dataContext)
    {
        this._dataContext = dataContext;
        GlobalConfiguration.Setup().UseMySql();
        MySqlConnectorBootstrap.Initialize();
        // _db = _dataContext.GetDbConnection();
    }
    
}

