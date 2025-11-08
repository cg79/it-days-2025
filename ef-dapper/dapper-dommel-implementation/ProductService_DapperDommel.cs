using ef_base_repository;
using ef_dapper_models;
using System.Data;
using Dapper;
using Dommel;

namespace dapper_dommel_implementation;

public partial class ProductService_Dommel: BaseConnection
{
    private IEFDataContext _dataContext;
    public ProductService_Dommel(IEFDataContext dataContext): base(dataContext)
    {
        this._dataContext = dataContext;
    }
}
