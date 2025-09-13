using ef_base_repository;
using ef_dapper_models;
using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;

namespace dapper_contrib_implementation;
public partial class UserService_DapperContrib : BaseConnection
{
    private IEFDataContext _dataContext;
    public UserService_DapperContrib(IEFDataContext dataContext): base(dataContext)
    {
        this._dataContext = dataContext;
    }
}

