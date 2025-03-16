namespace Domain.Entities.User;

using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

[Table("AspNetRoles", Schema = "User")]
public class Role : IdentityRole<Guid>
{

}