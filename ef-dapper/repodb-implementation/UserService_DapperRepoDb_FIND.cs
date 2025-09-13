using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using ef_base_repository;
using ef_dapper_models;
using RepoDb;

namespace repodb_implementation;

public partial class UserService_RepoDb
{
    public async Task<IEnumerable<User>> FindQueryFilter(QueryFilter filter)
    {
        var predicate = filter.ToExpression<User>();
        await using var db = await GetOpenConnectionAsync();

        var users = await db.QueryAsync<User>(predicate);

        return users;

    }
    public async Task<IEnumerable<User>> FindLinq( Expression<Func<User,bool>> whereExpression)
    {
        await using var db = await GetOpenConnectionAsync();

        var users = await db.QueryAsync<User>(whereExpression);

        return users;

    }

}

