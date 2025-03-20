namespace Infrastructure.Persistence;

using Domain.Entities.Basket;
using Domain.Entities.Order;
using Domain.Entities.Product;
using Domain.Entities.User;
using Domain.Shared.Configurations;
using Domain.Shared.Constants;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

//[DbContext(typeof(StoreContext))]
public class StoreContext(DbContextOptions options, IOptionsMonitor<ConnectionSettings> connetionSettings) 
	: IdentityDbContext<User, Role, Guid>(options)
{

	#region Store

	public DbSet<Basket> Baskets { get; set; }
	public DbSet<BasketItem> BasketItems { get; set; }
	public DbSet<Order> Orders { get; set; }
	public DbSet<OrderItem> OrderItems { get; set; }
	public DbSet<Product> Products { get; set; }

	#endregion

	#region User

	public DbSet<Address> Addresses { get; set; }
	public override DbSet<Role> Roles { get; set; }
	public override DbSet<User> Users { get; set; }

	#endregion

#pragma warning disable 612, 618

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.HasDefaultSchema(Constants.DbSchemaNameUser);

		base.OnModelCreating(builder);
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer(connetionSettings.CurrentValue.StoreContext,
			x =>
			{
				x.MigrationsHistoryTable("MigrationsHistory", Constants.DbSchemaNameApp);
				//x.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
			}).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

		base.OnConfiguring(optionsBuilder);
	}

#pragma warning restore 612, 618

}