namespace Infrastructure.Persistence;

using Domain.Entities.Basket;
using Domain.Entities.Order;
using Domain.Entities.Product;
using Domain.Entities.User;
using Domain.Shared.Configurations;
using Domain.Shared.Constants;

using Microsoft.AspNetCore.Identity;
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
	public DbSet<Permission> Permissions { get; set; }
	public override DbSet<Role> Roles { get; set; }
	public DbSet<RolePermission> RolePermissions { get; set; }
	public override DbSet<User> Users { get; set; }

	#endregion

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.HasDefaultSchema(DbConstants.DbSchemaNameUser);

		builder.Entity<IdentityUser>().ToTable(nameof(User));
		builder.Entity<IdentityRole>().ToTable(nameof(Role));

		builder.Entity<Role>()
			.HasMany(x => x.Permissions)
			.WithMany(x => x.Roles)
			.UsingEntity<RolePermission>();

		base.OnModelCreating(builder);
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer(connetionSettings.CurrentValue.StoreContext,
			x =>
			{
				x.MigrationsHistoryTable("MigrationsHistory", DbConstants.DbSchemaNameApp);
				//x.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
			}).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

		base.OnConfiguring(optionsBuilder);
	}
}