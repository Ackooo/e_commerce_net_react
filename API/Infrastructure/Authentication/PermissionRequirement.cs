namespace Infrastructure.Authentication;

using Microsoft.AspNetCore.Authorization;

public class PermissionRequirement(string p) : IAuthorizationRequirement
{
	public string Permission { get; } = p;
}
