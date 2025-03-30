namespace Domain.Entities.User;

using System.ComponentModel.DataAnnotations.Schema;

using Domain.Shared.Constants;

using Microsoft.AspNetCore.Identity;

[Table(DbConstants.IdentityRoleTableName, Schema = DbConstants.DbSchemaNameUser)]
public class Role : IdentityRole<Guid>
{

	#region NavigationProperies

	public ICollection<Permission> Permissions { get; set; } = [];
	public ICollection<User> Users { get; set; } = [];

	#endregion
}