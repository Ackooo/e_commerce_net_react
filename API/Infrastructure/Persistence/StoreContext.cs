namespace Infrastructure.Persistence;

using System.Reflection.Emit;

using Domain.Entities.Basket;
using Domain.Entities.Order;
using Domain.Entities.Product;
using Domain.Entities.User;
using Domain.Shared.Constants;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

//[DbContext(typeof(StoreContext))]
public class StoreContext(DbContextOptions options) : IdentityDbContext<User, Role, Guid>(options)
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
	public DbSet<Role> Roles { get; set; }
	public DbSet<User> Users { get; set; }

	#endregion

#pragma warning disable 612, 618

	protected override void OnModelCreating(ModelBuilder builder)
	{		
		builder.HasDefaultSchema("User");
		base.OnModelCreating(builder);
	}

#pragma warning restore 612, 618

}