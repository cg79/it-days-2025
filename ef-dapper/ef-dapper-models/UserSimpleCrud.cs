namespace ef_dapper_models;
using Dapper.Contrib.Extensions;

[Table("Users")]
public class UserSimpleCrud: IUser 
{
    [Key]
    public Int64 Id { get; set; }
    public int? Age { get; set; }
    public  string? FirstName { get; set; }
    public  string? LastName { get; set; }
    public  string? PhoneNumber { get; set; }
    public  string? Guid { get; set; }
    public required string Email { get; set; }
}