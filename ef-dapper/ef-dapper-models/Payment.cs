using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ef_dapper_models;

public class Payment : IRootEntity
{
    [Key]
    public long Id { get; set; }

    [Required]
    public long InvoiceId { get; set; }

    [Required]
    public DateTime PaymentDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [MaxLength(50)]
    public string Method { get; set; } // Card, BankTransfer, Cash

    [ForeignKey(nameof(InvoiceId))]
    public Invoice Invoice { get; set; }
}
