namespace Infrastructure.Authentication;

using System.Globalization;

using Domain.Shared.Constants;

using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class BaseAttribute : ActionFilterAttribute
{
	public override void OnActionExecuting(ActionExecutingContext context)
	{
		//TODO:
		//User info to handle in ControllerBase
		//var controller = context.Controller as ApiBaseController;
		// Localization move to url
		// Auth moved to separate attribute

		var language = context.HttpContext.User.Claims
			.Where(x => x.Type == CustomClaims.Language)
			.Select(x => x.Value).FirstOrDefault();

		var cultureInfo = language != null
			? new CultureInfo(language)
			: new CultureInfo(CultureInfos.English_US);

		CultureInfo.CurrentCulture = cultureInfo;
		CultureInfo.CurrentUICulture = cultureInfo;		

	}
}
