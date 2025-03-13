namespace API.Configuration;

using Domain.Interfaces.Services;

using Infrastructure.Services;

public static class DependencyConfiguration
{
    public static void AddDependencyConfiguration(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IImageService, ImageService>();
    }
}
