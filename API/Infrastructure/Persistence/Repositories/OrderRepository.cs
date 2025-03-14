namespace Infrastructure.Persistence.Repositories;

using Domain.Entities.Order;
using Domain.Interfaces.Repository;
using Domain.Shared.Enums;

using Microsoft.EntityFrameworkCore;

using Stripe;

public class OrderRepository(StoreContext storeContext) : IOrderRepository
{
	public Task<Order> GetByIdAsync(int id, string buyerId)
	{
		return storeContext.Orders.AsNoTracking()
			.Include(o => o.OrderItems)
			.Where(x => x.BuyerId == buyerId && x.Id == id)
			.FirstAsync();
	}

	public Task<List<Order>> GetByBuyerIdAsync(string buyerId)
	{
		return storeContext.Orders.AsNoTracking()
			.Include(o => o.OrderItems)
			.Where(x => x.BuyerId == buyerId)
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

		var order = await storeContext.Orders.FirstAsync(x => x.PaymentIntentId == paymentIntentId);
		order.OrderStatus = OrderStatus.PaymentReceived;
		await storeContext.SaveChangesAsync();
	}
}
