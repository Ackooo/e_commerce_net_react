namespace Infrastructure.Authentication;

using System.Globalization;
using System.Security.Claims;

using Domain.Shared.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class ApiBaseAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        HandleCultureInfo(context.HttpContext.Request);

        //TODO: return to next if async
        var claims = context.HttpContext.User.Claims;
        if(claims == null || !claims.Any()) return;

        var userIdClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid);
        if(userIdClaim == null) return;

        if(!Guid.TryParse(userIdClaim.Value, out var userId))
            return;

        if(context.Controller is not ApiBaseController controller) return;
        controller.CurrentUserId = userId;
    }

    //TODO: review potential Localization move to url
    private static void HandleCultureInfo(HttpRequest request)
    {
        var currentCulture = request.Cookies[CustomClaims.Culture]
            ?? CultureInfos.English_US;
        var cultureInfo = new CultureInfo(currentCulture);

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
    }

}