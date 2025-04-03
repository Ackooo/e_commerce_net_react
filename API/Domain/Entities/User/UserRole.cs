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

#pragma warning disable CS8618

    // Migration error due to additional FK added
    // public required User User { get; set; }

    public Role Role { get; set; }

#pragma warning restore CS8618

    #endregion
}
