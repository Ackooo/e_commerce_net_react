namespace Domain.Entities.User;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

[Table("AspNetUsers", Schema = "User")]
public class User : IdentityUser<Guid>
{

	[Column]
	[Required]
	[DefaultValue(0)]
	public byte Language { get; set; } = 0;

	#region NavigationProperies

	public Address? Address { get; set; }

	#endregion
}