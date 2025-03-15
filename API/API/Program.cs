using API.Configuration;
using API.Middleware;

using Domain.Entities.User;

using Infrastructure.Persistence;

using Microsoft.AspNetCore.Identity;

#region WebApplicationBuilder

var builder = WebApplication.CreateBuilder(args);

var configuration = SettingsConfiguration.AddConfiguration(args);

builder.Services.AddAppConfiguration(configuration);

builder.Services.AddControllers();

builder.Services.AddMappingConfiguration();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerConfiguration();

builder.Services.AddContextConfiguration(configuration);

builder.Services.AddCors();

builder.Services.AddIdentityConfiguration();

builder.Services.AddAuthenticationConfiguration(configuration);

builder.Services.AddDependencyConfiguration();

builder.Services.AddLocalizationConfiguration();

#endregion

#region WebApplication

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseSwaggerConfiguration();

app.UseCors(opt =>
{
    opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:3000");//credentials for cookies from dif domain
});

//allowcredentials for passing the cookie back and forth by client
//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

//app.UseAntiforgery();

app.MapControllers();

app.UseLocalizationConfiguration();

await app.InitializeDatabaseAsync();

await app.RunAsync();

#endregion