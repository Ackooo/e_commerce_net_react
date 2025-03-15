namespace Infrastructure.Services;

using Domain.Entities.User;
using Domain.Interfaces.Services;
using Domain.Shared.Configurations;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenService(UserManager<User> userManager, IOptionsMonitor<JwtSettings> jwtSettings) : ITokenService
{

    public async Task<string> GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName)

        };

        var roles = await userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.CurrentValue.TokenKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenOptions = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
            expires: DateTime.Now.AddDays(7), signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);

    }

}
