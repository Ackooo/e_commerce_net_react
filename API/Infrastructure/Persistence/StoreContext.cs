namespace Infrastructure.Persistence;

using Domain.Entities.Basket;
using Domain.Entities.Order;
using Domain.Entities.Product;
using Domain.Entities.User;
using Domain.Shared.Configurations;
using Domain.Shared.Constants;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public class StoreContext(DbContextOptions options, IOptionsMonitor<ConnectionSettings> connetionSettings)
	: DbContext(options)
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
	public DbSet<Role> Roles { get; set; }
	public DbSet<RolePermission> RolePermissions { get; set; }
	public DbSet<User> Users { get; set; }

	#endregion

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.Ignore<IdentityUserToken<Guid>>();
		builder.Ignore<IdentityUserRole<Guid>>();
		builder.Ignore<IdentityUserLogin<Guid>>();
		builder.Ignore<IdentityUserClaim<Guid>>();
		builder.Ignore<IdentityRoleClaim<Guid>>();

		builder.Entity<User>(x =>
		{
			x.Ignore(c => c.LockoutEnabled);
			x.Ignore(c => c.TwoFactorEnabled);
			x.Ignore(c => c.LockoutEnd);
			x.Ignore(c => c.PhoneNumberConfirmed);
		});

		builder.Entity<User>()
			.HasMany(x => x.Roles)
			.WithMany(x => x.Users)
			.UsingEntity<UserRole>();

		builder.Entity<Role>()
			.HasMany(x => x.Permissions)
			.WithMany(x => x.Roles)
			.UsingEntity<RolePermission>();

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