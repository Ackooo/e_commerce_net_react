namespace API.Configuration;

using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;

using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;

public static class DependencyConfiguration
{
    public static void AddDependencyConfiguration(this IServiceCollection services)
    {
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IBasketService, BasketService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();

    }
}
