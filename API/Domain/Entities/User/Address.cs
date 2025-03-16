namespace Domain.Entities.User;

using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(Address), Schema = "User")]
public class Address
{
    public long Id { get; set; }

    public required string FullName { get; set; }
    public required string Address1 { get; set; }
    public required string Address2 { get; set; }
    public required string Zip { get; set; }
    public required string City { get; set; }
    public string? State { get; set; }
    public required string Country { get; set; }

    public Guid UserId { get; set; }

}
