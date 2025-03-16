namespace Domain.Entities.User;

using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

[Table("AspNetUsers", Schema = "User")]
public class User : IdentityUser<Guid>
{    
    public Address? Address { get; set; }
}