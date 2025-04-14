namespace API.Middleware;

using System.Text.Json;

using Microsoft.AspNetCore.Mvc;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
{
    private static readonly JsonSerializerOptions options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            //TODO: review return messages logic
            logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;

            var response = new ProblemDetails
            {
                Status = 500,
                Detail = env.IsDevelopment() ? ex.StackTrace?.ToString() : null,
                Title = ex.Message
            };            

            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }

}