using ef_base_repository;
using ef_dapper_models;
using Dapper;
using System.Data;
using System.Text.Json;
using ef_implementation;

namespace dapper_implementation;

public partial class UserService_Dapper 
{

    public async Task<List<User>> FindUserAndInvoicesFromRawSQL(QueryFilter filter, string fields = "*")
    {
        var sql = @"
        SELECT u.Id, u.Email, u.FirstName, 
               i.Id, i.InvoiceDate, i.UserId, i.Status
        FROM Users AS u
        LEFT JOIN Invoices AS i ON u.Id = i.UserId AND i.Status = 'Paid'
        WHERE u.Id = @UserId;
    ";

        await using var db = await GetOpenConnectionAsync();

        var userDictionary = new Dictionary<long, User>();

        var users = await db.QueryAsync<User, Invoice, User>(
            sql,
            (user, invoice) =>
            {
                if (!userDictionary.TryGetValue(user.Id, out var userEntry))
                {
                    userEntry = user;
                    userEntry.Invoices = new List<Invoice>();
                    userDictionary.Add(userEntry.Id, userEntry);
                }

                if (invoice != null)
                {
                    userEntry.Invoices.Add(invoice);
                }

                return userEntry;
            },
            param: new { UserId = 6 },
            splitOn: "Id" // tells Dapper when to start mapping Invoice
        );

        return userDictionary.Values.ToList();
    }


   
}

