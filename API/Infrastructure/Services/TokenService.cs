namespace Infrastructure.Services;

using Domain.Entities.User;
using Domain.Exceptions;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;
using Domain.Shared.Configurations;
using Domain.Shared.Constants;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenService(UserManager<User> userManager, 
	IOptionsMonitor<JwtSettings> jwtSettings, IPermissionRepository permissionRepository) : ITokenService
{

	public async Task<string> GenerateTokenAsync(User user)
	{
		ValidateUser(user);
		var claims = new List<Claim>
		{
			//new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
			new(ClaimTypes.Email, user.Email!),
			new(ClaimTypes.Name, user.UserName!),
			new (ClaimTypes.Sid, user.Id.ToString()),
			new (CustomClaims.Language, user.Language.ToString())
		};

		var roles = await userManager.GetRolesAsync(user);
		foreach (var role in roles)
		{
			claims.Add(new Claim(ClaimTypes.Role, role));
		}

		var permissions = await permissionRepository.GetPermissionsAsync(user.Id);
		foreach (var permission in permissions)
		{
			claims.Add(new(CustomClaims.Permission, permission));
		}

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.CurrentValue.TokenKey));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

		var tokenOptions = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
			expires: DateTime.Now.AddDays(7), signingCredentials: creds);

		return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
	}

	private static void ValidateUser(User user)
	{
		if (string.IsNullOrWhiteSpace(user.UserName)
			|| string.IsNullOrWhiteSpace(user.Email)
			|| string.IsNullOrWhiteSpace(user.UserName))
			throw new ApiException("User Invalid");
	}

}
