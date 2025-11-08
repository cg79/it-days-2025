namespace ef_dapper.commands;

public class CreateUserCommand 
{ 
    public  string? FirstName { get; set; }
    public  string? LastName { get; set; }
    public  string? PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
        
}