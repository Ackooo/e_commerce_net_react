namespace Domain.Entities.User;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Domain.Shared.Constants;

using Microsoft.EntityFrameworkCore;

//[Index(nameof(RoleId), nameof(PermissionId), IsUnique = true, Name = $"UX_{nameof(RolePermission)}_{nameof(RoleId)}_{nameof(PermissionId)}")]
[Table(nameof(RolePermission), Schema = DbConstants.DbSchemaNameUser)]
[PrimaryKey(nameof(RoleId), nameof(PermissionId))]
public class RolePermission
{

	[Column(Order = 0)]
	[Required]
	[ForeignKey(nameof(Role))]
	public Guid RoleId { get; set; }

	[Column(Order = 1)]
	[Required]
	[ForeignKey(nameof(Permission))]
	public byte PermissionId { get; set; }

	#region NavigationProperies

	public required Role Role { get; set; }

	public required Permission Permission { get; set; }

	#endregion

}
