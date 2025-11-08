using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using ef_base_repository;
using ef_dapper_models;
using RepoDb;

namespace repodb_implementation;



public partial class UserService_RepoDb
{
    public async Task<List<User>> GetUserInvoicesMT(QueryFilter filter)
    {
        await using var db = await GetOpenConnectionAsync();

        var userPredicate = filter.ToExpression<User>();

        var users = await db.QueryAsync<User>(userPredicate);
        var invoices = await db.QueryAsync<Invoice>(i => i.Status == "Paid");

        var query =
            from u in users
            join o in invoices
                on u.Id equals o.UserId into userOrders
            select new User
            {
                Id = u.Id,
                Email = u.Email,
                Invoices = userOrders.ToList()
            };

        return query.ToList();

    }


}

