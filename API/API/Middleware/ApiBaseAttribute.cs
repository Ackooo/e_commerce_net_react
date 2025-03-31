namespace API.Middleware;

using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;

using API.Controllers.Common;

using Domain.Shared.Constants;

using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class ApiBaseAttribute : ActionFilterAttribute
{
	public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		var claims = context.HttpContext.User.Claims;
		if (claims == null || !claims.Any()) return;

		var userIdClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid);
		if (userIdClaim == null) return;

		if (!Guid.TryParse(userIdClaim.Value, out var userId))
		{
			return;
		}

		if (context.Controller is not ApiBaseController controller) return;

		controller.CurrentUserId = userId;

		//TODO: review with permissions refactor
		/*
		var userRepository = context.HttpContext.RequestServices.GetService<IUserRepository>()
			?? throw new ApiException();

		var user = await userRepository.GetUserWithPermissionsAsync(userId);
		if (user == null) return;

		controller.CurrentUser = user;
		*/

		HandleCultureInfo(claims, controller);

		await next();
	}

	//TODO: review potential Localization move to url
	private static void HandleCultureInfo(IEnumerable<Claim> claims, ApiBaseController controller)
	{
		var language = claims
			.Where(x => x.Type == CustomClaims.Language)
			.Select(x => x.Value)
			.FirstOrDefault();

		var cultureInfo = language != null
			? new CultureInfo(language)
			: new CultureInfo(CultureInfos.English_US);

		CultureInfo.CurrentCulture = cultureInfo;
		CultureInfo.CurrentUICulture = cultureInfo;

		controller.CultureInfo = cultureInfo;

	}

}
