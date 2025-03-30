namespace Infrastructure.Authentication;

using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

public class PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
	: DefaultAuthorizationPolicyProvider(options)
{
	public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
	{
		var policy = await base.GetPolicyAsync(policyName);
		if (policy is not null)
		{
			return policy;
		}

		//Automatically define missing policies for when we define new permission enum values
		return new AuthorizationPolicyBuilder()
			.AddRequirements(new PermissionRequirement(policyName))
			.Build();
	}

}
