namespace Infrastructure.Persistence;

using Domain.Entities.Basket;
using Domain.Entities.Order;
using Domain.Entities.Product;
using Domain.Entities.User;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

//[DbContext(typeof(StoreContext))]
public class StoreContext : IdentityDbContext<User, Role, Guid>
{
    public StoreContext(DbContextOptions options) : base(options)
    {

    }
    
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<BasketItem> BasketItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
	public DbSet<Product> Products { get; set; }
	public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Address> Addresses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
		
        builder.Entity<User>()
            .HasOne(a => a.Address)
            .WithOne()
            .HasForeignKey<Address>(a => a.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
