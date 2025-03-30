namespace Domain.Entities.User;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Domain.Shared.Constants;

[Table(nameof(Permission), Schema = DbConstants.DbSchemaNameUser)]
public class Permission
{

	[Column]
	[Required]
	[Key]
	public byte Id { get; set; }

	[Column]
	[Required]
	[MaxLength(64)]
	public required string Name { get; set; }

	#region NavigationProperies

	public ICollection<Role> Roles { get; set; } = [];

	#endregion

}
