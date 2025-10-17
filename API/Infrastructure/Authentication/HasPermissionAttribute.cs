namespace Infrastructure.Authentication;

using System.Net;
using Domain.Exceptions;
using Domain.Interfaces.Repository;
using Domain.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class HasPermissionAttribute(Permissions[] mandatoryPermissions) : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        try
        {
            if(context.Controller is not ApiBaseController controller)
            {
                await CreateUnauthorizedResponse(context);
                return;
            }

            var userRepository = context.HttpContext.RequestServices.GetService<IUserRepository>()
                ?? throw new ApiException();

            var user = await userRepository.GetUserWithPermissionsAsync(controller.CurrentUserId!.Value);
            if(user == null)
            {
                await CreateUnauthorizedResponse(context);
                return;
            }

            var userPermisisons = user.Roles.SelectMany(x => x.Permissions)
                .Select(x => x.Id).ToList();

            if(!HasAnyPermission(userPermisisons, mandatoryPermissions))
            {
                await CreateUnauthorizedResponse(context);
                return;
            }
            
            controller.CurrentUser = user;
            await next();
        }
        catch
        {
            await CreateUnauthorizedResponse(context);
            return;
        }
    }

    #region Helper

    private static async Task CreateUnauthorizedResponse(ActionExecutingContext context)
    {
        context.HttpContext.Response.ContentType = "application/json";
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden; // maybe 418 :)
        await context.HttpContext.Response.WriteAsync("You are not Aleksa!");
    }

    private static bool HasAnyPermission(List<byte>? userPermissions, Permissions[] mandatoryPermissions)
    {
        if(userPermissions == null || userPermissions.Count == 0) return false;

        var mandateoryPermissionsByte =
            Array.ConvertAll(mandatoryPermissions, value => (byte)value);

        return userPermissions.Any(u => mandateoryPermissionsByte.Contains(u));
    }

    #endregion

}