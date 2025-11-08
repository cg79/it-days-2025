using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using ef_base_repository;
using ef_dapper_models;
using RepoDb;

namespace repodb_implementation;

public partial class ProductService_RepoDb
{
    public async Task<IEnumerable<Products>> FindQueryFilter(QueryFilter filter)
    {
        var predicate = filter.ToExpression<Products>();
        await using var db = await GetOpenConnectionAsync();

        var users = await db.QueryAsync<Products>(predicate);

        return users;

    }
    public async Task<IEnumerable<Products>> FindLinq( Expression<Func<Products,bool>> whereExpression)
    {
        await using var db = await GetOpenConnectionAsync();

        var users = await db.QueryAsync<Products>(whereExpression);

        return users;

    }

}

