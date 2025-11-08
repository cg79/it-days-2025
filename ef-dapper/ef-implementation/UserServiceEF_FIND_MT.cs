using System.Linq.Expressions;
using ef_base_repository;
using ef_dapper_models;
using LinqToDB;

namespace ef_implementation;

public partial class UserServiceEF 
{
    public async Task<List<User>> GetUserInvoicesMT(QueryFilter filter)
    {
            GenericRepository<User> userRepo = new GenericRepository<User>(_dataContext);
            GenericRepository<Invoice> invoiceRepository = new GenericRepository<Invoice>(_dataContext);

            var users = userRepo.FindQueryFilter(filter);
            var invoices = invoiceRepository.FindLinq(
                el => el.Status == "Paid");

            var query =
                from u in users
                join o in invoices
                    on u.Id equals o.UserId into userOrders // group join
                select new User
                {
                    Id = u.Id,
                    Email = u.Email,
                    Invoices = userOrders.ToList()
                };

            var results = await query.ToListAsync();
            return results;
    }

     
    public async Task<List<User>> GetUsers_InvoicesAsyncMultipleQueries(QueryFilter filter)
    {
        GenericRepository<User> userRepo = new GenericRepository<User>(_dataContext);
        GenericRepository<Invoice> invoiceRepository = new GenericRepository<Invoice>(_dataContext);           
    
        var users = await userRepo.FindQueryFilter(filter).ToListAsync();
        var userIds = users.Select(u => u.Id).ToList();
        var invoices = await invoiceRepository.FindLinq(el => userIds.Contains(el.UserId)).ToListAsync();
        var invoicesByUserId = invoices
            .GroupBy(p => p.UserId)
            .ToDictionary(g => g.Key, g => g.ToList());
        foreach (var user in users)
        {
            if (invoicesByUserId.TryGetValue(user.Id, out var invoiceList))
            {
                user.Invoices = invoiceList;
            }
        }
        return users;
        
    }
    
    
    


}
