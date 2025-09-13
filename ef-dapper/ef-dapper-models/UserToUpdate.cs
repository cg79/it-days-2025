
using System.ComponentModel.DataAnnotations;

namespace ef_dapper_models;

[System.ComponentModel.DataAnnotations.Schema.Table("Users")] // optional, if your table name != class name
public class UserToUpdate
{
    [Key] // SimpleCRUD needs a primary key
    public int Id { get; set; }

    public string FirstName { get; set; } = "";
    public string Email { get; set; } = "";
}