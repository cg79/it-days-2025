using ef_base_repository;
using ef_dapper_models;
using System.Data;
using System.Linq.Expressions;
using Dommel;

namespace dapper_dommel_implementation;

public partial class ProductService_Dommel: BaseConnection
{
    
    public async Task<IEnumerable<Products>> FindQueryFilterToPredicate(QueryFilter filter)
    {
        var expression = filter.ToExpression<Products>();

        await using var db = await GetOpenConnectionAsync();
        var filteredUsers = await db.SelectAsync<Products>(expression);
        
        return filteredUsers.ToList();
    }
    
    public async Task<IEnumerable<Products>> FindLinq(Expression<Func<Products, bool>> expression)
    {
        await using var db = await GetOpenConnectionAsync();
        var filteredUsers = await db.SelectAsync<Products>(expression);
        
        return filteredUsers;
    }

}
