using LinqToDB.Mapping;

namespace ef_dapper_models;

[Table(Name = "Users", IsColumnAttributeRequired = false)]
public class UserLinqToDb : User,IUser
{
    // [PrimaryKey, Identity, Key]         
    // // [Column(Name = "Id")] 
    // public Int64 Id { get; set; }
    //
    // public int? Age { get; set; }
    //
    // public  string? FirstName { get; set; }
    // public  string? LastName { get; set; }
    // public  string? PhoneNumber { get; set; }
    // public  string? Guid { get; set; }
    //
    // [Column(Name = "Email"), Nullable]
    // public required string Email { get; set; }
}