namespace Domain.Entities.User;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(Address), Schema = "User")]
public class Address
{
	[Key]
	public long Id { get; set; }

	[Required]
	[MaxLength(256)]
	public required string FullName { get; set; }

	[Required]
	[MaxLength(256)]
	public required string Address1 { get; set; }

	[Required]
	[MaxLength(256)]
	public required string Address2 { get; set; }

	[Required]
	[MaxLength(256)]
	public required string Zip { get; set; }

	[Required]
	[MaxLength(256)]
    public required string City { get; set; }

	[MaxLength(256)]
	public string? State { get; set; }

	[Required]
	[MaxLength(256)]
	public required string Country { get; set; }

	[ForeignKey(nameof(User))]
    public Guid? UserId { get; set; }

	#region NavigationProperies

	public User? User { get; set; }

	#endregion

}
