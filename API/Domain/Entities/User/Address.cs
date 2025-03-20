namespace Domain.Entities.User;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(Address), Schema = "User")]
public class Address
{

	[Column]
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }

	[Column]
	[Required]
	[MaxLength(256)]
	public required string FullName { get; set; }

	[Column]
	[Required]
	[MaxLength(256)]
	public required string Address1 { get; set; }

	[Column]
	[Required]
	[MaxLength(256)]
	public required string Address2 { get; set; }

	[Column]
	[Required]
	[MaxLength(256)]
	public required string Zip { get; set; }

	[Column]
	[Required]
	[MaxLength(256)]
    public required string City { get; set; }

	[Column]
	[MaxLength(256)]
	public string? State { get; set; }

	[Column]
	[Required]
	[MaxLength(256)]
	public required string Country { get; set; }

	[Column]
	[ForeignKey(nameof(User))]
    public Guid? UserId { get; set; }

	#region NavigationProperies

	public User? User { get; set; }

	#endregion

}
