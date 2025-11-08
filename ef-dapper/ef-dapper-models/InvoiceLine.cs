using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ef_dapper_models;

public class InvoiceLine
{
    [Key]
    public long Id { get; set; }

    [Required]
    public long InvoiceId { get; set; }

    [Required, MaxLength(200)]
    public string Description { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal LineTotal => Quantity * UnitPrice;

    // Navigation
    [ForeignKey(nameof(InvoiceId))]
    public Invoice Invoice { get; set; }
}
