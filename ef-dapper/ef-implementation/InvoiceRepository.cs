using System.Linq.Dynamic.Core;
using ef_base_repository;
using ef_dapper_models;
using Microsoft.EntityFrameworkCore;

namespace ef_implementation;

public class InvoiceRepository(IEFDataContext context) : GenericRepository<Invoice>(context)
{
    public async Task<List<Invoice>> GetInvoicesAsync(QueryFilter group)
    {
        try
        {
            var query = Find(group)
                .AsNoTracking()               // optional: no tracking for read-only
                .Include(i => i.Payment)      // include associated payment
                .Include(i => i.InvoiceLines) // include invoice lines
                .Include(i => i.TimeBills);   // include time bills

            return await query.ToListAsync(); // async version
        }
        catch (Exception ex)
        {
            throw; // no need to "throw ex" (preserves stack trace)
        }
    }
    
    public async Task<List<Invoice>> GetInvoicesAsyncMultipleQueries(QueryFilter group)
    {
        try
        {
            var invoices = await Find(group)
                .AsNoTracking().ToListAsync();               
                // .Include(i => i.Payment)      // include associated payment
                // .Include(i => i.InvoiceLines) // include invoice lines
                // .Include(i => i.TimeBills);   // include time bills
    
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
            // return await invoices.ToListAsync(); // async version
        }
        catch (Exception ex)
        {
            throw; // no need to "throw ex" (preserves stack trace)
        }
    }
}