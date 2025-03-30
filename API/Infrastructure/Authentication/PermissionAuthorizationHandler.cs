namespace Infrastructure.Authentication;

using System.Threading.Tasks;

using Domain.Shared.Constants;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

public class PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory) 
	: AuthorizationHandler<PermissionRequirement>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
		PermissionRequirement requirement)
	{
		/*
		//var memberId = context.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
		var userId = context.User.Claims.First(x => x.Type == ClaimTypes.Sid).Value;
		//TODO: check for context.Fail()
		if (userId == null) return;

		if(!Guid.TryParse(userId, out var parsedMemberId))
		{
			return;
		}

		using var scope = serviceScopeFactory.CreateScope();
		var permissionRepository = scope.ServiceProvider
			.GetRequiredService<IPermissionRepository>();

		//TODO: to cache
		//var permissions = await permissionRepository.GetPermissionsAsync(parsedMemberId);
		*/

		//TODO: 
		//Handle jwt size
		//Revoke token when permission is changed

		var permissions = context.User.Claims
			.Where(x => x.Type == CustomClaims.Permission)
			.Select(x => x.Value)
			.ToHashSet();
		
		if (permissions.Contains(requirement.Permission))
		{
			context.Succeed(requirement);
		}
		
		return Task.CompletedTask;
	}
}
