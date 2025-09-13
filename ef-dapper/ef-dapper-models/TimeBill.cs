using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ef_dapper_models;

public class TimeBill
{
    [Key]
    public long Id { get; set; }

    [Required]
    public long InvoiceId { get; set; }

    [Required]
    public DateTime WorkDate { get; set; }

    public decimal Hours { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal RatePerHour { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total => Hours * RatePerHour;

    [ForeignKey(nameof(InvoiceId))]
    public Invoice Invoice { get; set; }
}
