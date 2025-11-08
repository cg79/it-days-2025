using ef_base_repository;
using ef_dapper_models;
using Dapper;
using System.Data;
using System.Text.Json;
using ef_implementation;

namespace dapper_implementation;

public partial class ProductService_Dapper : BaseConnection
{
    
    private IEFDataContext _dataContext;
    public ProductService_Dapper(IEFDataContext dataContext):base(dataContext)
    {
        this._dataContext = dataContext;
    }

    public async Task<IEnumerable<User>> Find(QueryFilter filter, string fields = "*")
    {
        var (whereClause, parameters) = filter.ToSqlWithParams();
        string sql = $"SELECT {fields} FROM Products WHERE {whereClause}";

        await using var db = await GetOpenConnectionAsync();
        var filteredUsers = await db.QueryAsync<User>(sql, parameters);
        
        return filteredUsers;
    }

    public async Task<IEnumerable<Products>> FindProductsWithSP(string name, decimal price)
    {
        await using var db = await GetOpenConnectionAsync();
        
        var filteredUsers = await db.QueryAsync<Products>(
            "FindProductsByNameAndAge",
            new { p_Name = name, p_Price = price },
            commandType: CommandType.StoredProcedure
        );

        return filteredUsers;
    }
    
    
}



