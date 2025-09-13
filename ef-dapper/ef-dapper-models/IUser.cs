namespace ef_dapper_models;

public interface IRootEntity
{
    public Int64 Id { get; set; }
}
public interface IUser : IRootEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Guid { get; set; }
    public int? Age { get; set; }
}