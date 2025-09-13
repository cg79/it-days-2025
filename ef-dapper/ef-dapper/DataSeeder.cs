using ef_base_repository;
using ef_dapper_models;

namespace ef_dapper;

using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class DataSeeder
{
    public static async Task SeedAsync(DataContext db, int count = 10_000)
    {
        // clear old data
        await db.Database.ExecuteSqlRawAsync("DELETE FROM Payments");
        await db.Database.ExecuteSqlRawAsync("DELETE FROM TimeBills");
        await db.Database.ExecuteSqlRawAsync("DELETE FROM InvoiceLines");
        await db.Database.ExecuteSqlRawAsync("DELETE FROM Invoices");
        await db.Database.ExecuteSqlRawAsync("DELETE FROM Users");

        var userFaker = new Faker<User>()
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.Guid, f => Guid.NewGuid().ToString())
            .RuleFor(u => u.Age, f => f.Random.Int(2, 100))
            .RuleFor(u => u.IsActive, f => f.Random.Bool());

        var users = userFaker.Generate(count);
        await db.Users.AddRangeAsync(users);
        await db.SaveChangesAsync();

        var invoiceId = 1L;
        var rand = new Random();

        var invoices = new List<Invoice>();
        var lines = new List<InvoiceLine>();
        var timeBills = new List<TimeBill>();
        var payments = new List<Payment>();

        foreach (var user in users)
        {
            int invoiceCount = rand.Next(1, 5); // 1-4 invoices per user
            for (int i = 0; i < invoiceCount; i++)
            {
                var invoice = new Invoice
                {
                    UserId = user.Id,
                    InvoiceDate = DateTime.UtcNow.AddDays(-rand.Next(1, 365)),
                    Status = rand.Next(2) == 0 ? "Paid" : "Draft",
                    TotalAmount = 0m
                };
                invoices.Add(invoice);

                // invoice lines
                int lineCount = rand.Next(1, 5);
                for (int j = 0; j < lineCount; j++)
                {
                    var qty = rand.Next(1, 10);
                    var price = (decimal)(rand.Next(10, 500));
                    var line = new InvoiceLine
                    {
                        Invoice = invoice,
                        Description = $"Service {j + 1}",
                        Quantity = qty,
                        UnitPrice = price
                    };
                    lines.Add(line);
                    invoice.TotalAmount += line.LineTotal;
                }

                // time bills
                int tbCount = rand.Next(0, 3);
                for (int k = 0; k < tbCount; k++)
                {
                    var hours = (decimal)(rand.NextDouble() * 8);
                    var rate = (decimal)(rand.Next(50, 200));
                    var tb = new TimeBill
                    {
                        Invoice = invoice,
                        WorkDate = DateTime.UtcNow.AddDays(-rand.Next(1, 180)),
                        Hours = Math.Round(hours, 2),
                        RatePerHour = rate
                    };
                    timeBills.Add(tb);
                    invoice.TotalAmount += tb.Total;
                }

                // payments
                if (invoice.Status == "Paid")
                {
                    payments.Add(new Payment
                    {
                        Invoice = invoice,
                        PaymentDate = DateTime.UtcNow.AddDays(-rand.Next(1, 180)),
                        Amount = invoice.TotalAmount,
                        Method = rand.Next(2) == 0 ? "Card" : "BankTransfer"
                    });
                }

                invoiceId++;
            }
        }

        await db.Invoices.AddRangeAsync(invoices);
        await db.InvoiceLines.AddRangeAsync(lines);
        await db.TimeBills.AddRangeAsync(timeBills);
        await db.Payments.AddRangeAsync(payments);

        await db.SaveChangesAsync();
    }
}
