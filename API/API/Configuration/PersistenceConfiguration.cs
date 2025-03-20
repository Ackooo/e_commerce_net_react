namespace API.Configuration;

using Domain.Shared.Configurations;
using Domain.Shared.Constants;

using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public static class PersistenceConfiguration
{
	public static void AddContextConfiguration(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<StoreContext>(opt =>
		opt.UseSqlServer(configuration.GetSection(nameof(ConnectionSettings))
			.GetValue<string>(nameof(StoreContext)), x =>
		{
			//x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
		}).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
		);
	}

	public static void EnsureInitialDbConfig(this WebApplication app)
	{
		using (var scope = app.Services.CreateScope())
		{
			var service = scope.ServiceProvider;
			var context = service.GetService<StoreContext>();
			var command = $" IF (SCHEMA_ID('{Constants.DbSchemaNameApp}') IS NULL) " +
				$"BEGIN EXEC ('CREATE SCHEMA [{Constants.DbSchemaNameApp}] AUTHORIZATION [dbo]') END";
			context!.Database.ExecuteSqlRaw(command);
		}
	}

}
