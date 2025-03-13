namespace Domain.Entities.User;

using Microsoft.AspNetCore.Identity;

public class User : IdentityUser<int>
{
    public UserAddess Address { get; set; }
}

