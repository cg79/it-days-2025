namespace ef_dapper_models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions; 
using LinqToDB.Mapping;

[System.ComponentModel.DataAnnotations.Schema.Table("Users")]
// [LinqToDB.Mapping.Table(Name = "Users")]
public class User: IUser 
{
    [System.ComponentModel.DataAnnotations.Key, PrimaryKey, Identity]
    public Int64 Id { get; set; }
    
    public int? Age { get; set; }
    public  string? FirstName { get; set; }
    public  string? LastName { get; set; }
    public  string? PhoneNumber { get; set; }
    public  string? Guid { get; set; }
    
    public bool? IsActive { get; set; }
    
    [LinqToDB.Mapping.Column(Name = "Email"), Nullable]
    public required string Email { get; set; }
    
    [Write(false)] 
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}