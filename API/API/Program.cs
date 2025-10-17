using API.Configuration;
using API.Middleware;

#region WebApplicationBuilder

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider((context, options) =>
{
	options.ValidateScopes = true;
	options.ValidateOnBuild = true;
});

var configuration = SettingsConfiguration.AddConfiguration(args);

builder.Services.AddAppConfiguration(configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerConfiguration();

builder.Services.AddContextConfiguration(configuration);

builder.Services.AddCors();

builder.Services.AddIdentityConfiguration();

builder.Services.AddAuthorizationConfiguration(configuration);

builder.Services.AddLocalizationConfiguration();

builder.Services.AddDependencyConfiguration();

#endregion

#region WebApplication

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseSwaggerConfiguration();

app.UseCorsConfiguration(configuration);

app.UseAuthentication();

app.UseAuthorization();

//TODO:
//app.UseAntiforgery();

app.MapControllers();

app.UseLocalizationConfiguration();

await app.RunAsync();

#endregion