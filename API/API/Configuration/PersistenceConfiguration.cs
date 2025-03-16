namespace API.Configuration;

using System.Threading.Tasks;

using Db.Data;

using Domain.Entities.User;

using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;

public static class PersistenceConfiguration
{
    public static void AddContextConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        //TODO:
        //Reconfig after query refactor
        services.AddDbContext<StoreContext>(opt =>
        opt.UseSqlServer(configuration.GetValue<string>(nameof(StoreContext)), x =>
        {            
            //x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        })//.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
        );
    }

    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            await context.Database.MigrateAsync();
            await DbInitializer.Initialize(context, userManager);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "A problem occured during migration");
        }

    }
}
