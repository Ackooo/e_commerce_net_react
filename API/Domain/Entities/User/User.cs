namespace Domain.Entities.User;

using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

[Table("AspNetUsers", Schema = "User")]
public class User : IdentityUser<Guid>
{
	#region NavigationProperies

	public Address? Address { get; set; }

	#endregion
}