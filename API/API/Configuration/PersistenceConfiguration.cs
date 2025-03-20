namespace API.Configuration;

using Domain.Shared.Configurations;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public static class PersistenceConfiguration
{
    public static void AddContextConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        //TODO:
        //Reconfig after query refactor
        services.AddDbContext<StoreContext>(opt =>
        opt.UseSqlServer(configuration.GetSection(nameof(ConnectionSettings)).GetValue<string>(nameof(StoreContext)), x =>
        {            
            //x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        })//.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
        );
    }
}
