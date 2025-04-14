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
	public DbSet<UserRole> UserRoles { get; set; }

	#endregion

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Ignore<IdentityUserToken<Guid>>();
		modelBuilder.Ignore<IdentityUserRole<Guid>>();
		modelBuilder.Ignore<IdentityUserLogin<Guid>>();
		modelBuilder.Ignore<IdentityUserClaim<Guid>>();
		modelBuilder.Ignore<IdentityRoleClaim<Guid>>();

		modelBuilder.Entity<User>(x =>
		{
			x.Ignore(c => c.LockoutEnabled);
			x.Ignore(c => c.TwoFactorEnabled);
			x.Ignore(c => c.LockoutEnd);
			x.Ignore(c => c.PhoneNumberConfirmed);
		});

		modelBuilder.Entity<User>()
			.HasMany(x => x.Roles)
			.WithMany(x => x.Users)
			.UsingEntity<UserRole>();

		modelBuilder.Entity<Role>()
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