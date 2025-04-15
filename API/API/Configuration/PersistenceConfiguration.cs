namespace API.Configuration;

using Domain.Shared.Configurations;

using Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

public static class PersistenceConfiguration
{
    public static void AddContextConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<StoreContext>(opt =>
            opt.UseSqlServer(configuration.GetSection(nameof(ConnectionSettings))
                .GetValue<string>(nameof(StoreContext)), sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null// Let EF handle default transient errors
                        );
                }));

        //Moved to StoreContext
        //services.AddDbContext<StoreContext>(opt =>
        //opt.UseSqlServer(configuration.GetSection(nameof(ConnectionSettings))
        //	.GetValue<string>(nameof(StoreContext)), x =>
        //{
        //	//x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        //}).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
        //);
    }
}
