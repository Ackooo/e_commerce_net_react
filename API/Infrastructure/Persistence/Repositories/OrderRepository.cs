namespace Infrastructure.Persistence.Repositories;

using Domain.Entities.Order;
using Domain.Interfaces.Repository;
using Domain.Shared.Enums;

using Microsoft.EntityFrameworkCore;

public class OrderRepository(StoreContext storeContext) : IOrderRepository
{
	public Task<Order> GetByIdAsync(Guid id, Guid userId, bool isTracked = false)
	{
		var query = isTracked
			? storeContext.Orders.AsTracking()
			: storeContext.Orders.AsNoTracking();

		return query
			.Include(o => o.OrderItems)
			.Where(x => x.UserId == userId && x.Id == id)
			.FirstAsync();
	}

	public Task<List<Order>> GetByBuyerIdAsync(Guid userId)
	{
		return storeContext.Orders.AsNoTracking()
			.Include(o => o.OrderItems)
			.Where(x => x.UserId == userId)
			.ToListAsync();
	}

	public async Task<Order> AddOrderAsync(Order order)
	{
		await storeContext.Orders.AddAsync(order);
		await storeContext.SaveChangesAsync();
		return order;
	}

	public async Task UpdateOrderStatusAsync(string paymentIntentId, string chargeStatus)
	{
		if (chargeStatus != "succeeded") return;

		var order = await storeContext.Orders.AsTracking()
			.FirstAsync(x => x.PaymentIntentId == paymentIntentId);
		order.OrderStatus = OrderStatus.PaymentReceived;
		await storeContext.SaveChangesAsync();
	}
}
