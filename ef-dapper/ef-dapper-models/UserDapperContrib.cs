namespace ef_dapper_models;
using Dapper.Contrib.Extensions;

[Table("Users")]
public class UserDapperContrib 
{
    [Key]
    public Int64  Id { get; set; }
    public  string? FirstName { get; set; }
    public  string? LastName { get; set; }
    public required string Email { get; set; }
}