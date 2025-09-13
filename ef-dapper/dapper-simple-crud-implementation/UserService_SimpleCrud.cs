using ef_base_repository;
using ef_dapper_models;
using MySql.Data.MySqlClient;
using System.Data;
using Dapper;

namespace dapper_simple_crud_implementation;

public partial class UserService_SimpleCrud : BaseConnection
{
    private IEFDataContext _dataContext;
    public UserService_SimpleCrud(IEFDataContext dataContext): base(dataContext)
    {
        this._dataContext = dataContext;
        SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);
    }

}
