using ef_base_repository;
using ef_dapper_models;
using Dapper;
using System.Data;
using System.Text.Json;
using ef_implementation;

namespace dapper_implementation;

public partial class UserService_Dapper : BaseConnection
{
    private IEFDataContext _dataContext;
    public UserService_Dapper(IEFDataContext dataContext):base(dataContext)
    {
        this._dataContext = dataContext;
    }
}



