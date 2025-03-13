namespace API.Configuration;

using Domain.Entities.User;

using Infrastructure.Persistence;

public static class IdentityConfiguration
{
    public static void AddIdentityConfiguration(this IServiceCollection services)
    {
        services.AddIdentityCore<User>(opt =>
        {
            opt.User.RequireUniqueEmail = true;

        })
    .AddRoles<Role>()
    .AddEntityFrameworkStores<StoreContext>();

    }
}
