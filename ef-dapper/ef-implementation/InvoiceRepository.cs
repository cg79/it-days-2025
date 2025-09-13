using System.Linq.Dynamic.Core;
using ef_base_repository;
using ef_dapper_models;
using Microsoft.EntityFrameworkCore;

namespace ef_implementation;

public class InvoiceRepository(IEFDataContext context) : GenericRepository<Invoice>(context)
{
    public async Task<List<Invoice>> GetInvoicesAsync(QueryFilter group)
    {
            var query = FindQueryFilter(group)
                .AsNoTracking()               
                .Include(i => i.Payment)      
                .Include(i => i.InvoiceLines) 
                .Include(i => i.TimeBills);   

            return await query.ToListAsync(); // async version
    }
    
    public async Task<List<Invoice>> GetInvoicesAsyncMultipleQueries(QueryFilter group)
    {
            var invoices = await FindQueryFilter(group)
                .AsNoTracking().ToListAsync();               
    
           var invoiceIds = invoices.Select(i => i.Id).ToList();
           var payments = await _context.Set<Payment>()
               .AsNoTracking()
               .Where(p => invoiceIds.Contains(p.InvoiceId))
               .Select(p => new Payment()
               {
                   InvoiceId = p.InvoiceId,
                   PaymentDate = p.PaymentDate,
                   Amount = p.Amount
               })
               .ToListAsync();
           
           var paymentsByInvoiceId = payments
               .GroupBy(p => p.InvoiceId)
               .ToDictionary(g => g.Key, g => g.ToList());
           
           foreach (var invoice in invoices)
           {
               if (paymentsByInvoiceId.TryGetValue(invoice.Id, out var paymentList))
               {
                   invoice.Payments = paymentList;
               }
           }

           return invoices;
        
    }
}