using LinqToDB.Mapping;

namespace ef_dapper_models;

[Table(Name = "Users", IsColumnAttributeRequired = false)]
public class UserLinqToDb_CustomFields 
{
    [System.ComponentModel.DataAnnotations.Key, PrimaryKey, Identity]
    public Int64 Id { get; set; }
    
    public int? Age { get; set; }
    
    [Column(Name = "Email"), Nullable]
    public  string Email { get; set; }
}