namespace Domain.Entities.User;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Domain.Shared.Constants;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

[Table(nameof(User), Schema = DbConstants.DbSchemaNameUser)]
//[Table(Constants.IdentityUserTableName, Schema = Constants.DbSchemaNameUser)]
[Index(nameof(NormalizedEmail), IsUnique = true,
	Name = $"UX_{DbConstants.IdentityUserTableName}_{nameof(NormalizedEmail)}")]
[Index(nameof(NormalizedUserName), IsUnique = true,
	Name = $"UX_{DbConstants.IdentityUserTableName}_{nameof(NormalizedUserName)}")]
public class User : IdentityUser<Guid>
{

	[Column]
	[Required]
	[MaxLength(5)]
	public string Language { get; set; } = CultureInfos.English_US;

	#region NavigationProperies

	public Address? Address { get; set; }

	public ICollection<Role> Roles { get; set; } = [];

	#endregion
}