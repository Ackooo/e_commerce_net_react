namespace Domain.Entities.User;

public class Address
{
    public required string FullName { get; set; }
    public required string Address1 { get; set; }
    public required string Address2 { get; set; }
    public required string Zip { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string Country { get; set; }

}
