namespace API.Configuration;

using System.Text;
using Domain.Shared.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

public static class AuthenticationConfiguration
{
    public static void AddAuthorizationConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenKey = configuration
            .GetSection(nameof(JwtSettings))
             .GetValue<string>(nameof(JwtSettings.TokenKey));
        if(string.IsNullOrWhiteSpace(tokenKey)) throw new ArgumentNullException(nameof(configuration));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey))
            };
        });

        services.AddAuthorization();

        //left for reference
        //services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        //services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
    }

    public static void UseCorsConfiguration(this WebApplication app, IConfiguration configuration)
    {
        var origin = configuration
            .GetSection(nameof(ConnectionSettings))
            .GetValue<string>("ClientApp");
        if(string.IsNullOrWhiteSpace(origin)) throw new ArgumentNullException(nameof(configuration));

        app.UseCors(opt =>
        {
            opt
            .WithOrigins(origin)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });

        app.UseHttpsRedirection();
    }

}
