namespace Domain.Entities.User;

using Shared.Constants;

using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(UserRole), Schema = DbConstants.DbSchemaNameUser)]
[PrimaryKey(nameof(UserId), nameof(RoleId))]
public class UserRole
{
	[Column(Order = 0)]
	[Required]
	[ForeignKey(nameof(User))]
	public Guid UserId { get; set; }

	[Column(Order = 1)]
	[Required]
	[ForeignKey(nameof(Role))]
	public Guid RoleId { get; set; }

	#region NavigationProperies

	// Migration error due to additional FK added
	// public required User User { get; set; }

	public required Role Role { get; set; }

	#endregion
}
