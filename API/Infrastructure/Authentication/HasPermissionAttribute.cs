namespace Infrastructure.Authentication;

using Domain.Shared.Enums;

using Microsoft.AspNetCore.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class HasPermissionAttribute : AuthorizeAttribute
{
	public HasPermissionAttribute(Permissions permission) 
		: base(policy: permission.ToString())
	{
		
	}

}
