using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ef_dapper_models;


[System.ComponentModel.DataAnnotations.Schema.Table("Invoices")]
public class Invoice : IRootEntity
{
    [Key]
    public long Id { get; set; }

    [Required]
    public long UserId { get; set; }

    [Required]
    public DateTime InvoiceDate { get; set; }

    [Required, MaxLength(50)]
    public string Status { get; set; } = "Draft"; // Draft, Sent, Paid, Overdue

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    // Navigation
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

    public ICollection<InvoiceLine> InvoiceLines { get; set; } = new List<InvoiceLine>();

    public ICollection<TimeBill> TimeBills { get; set; } = new List<TimeBill>();

    public Payment Payment { get; set; } // One-to-one optional
    public List<Payment> Payments { get; set; }
}
