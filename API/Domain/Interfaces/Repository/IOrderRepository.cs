namespace Domain.Interfaces.Repository;

using Domain.Entities.Order;

public interface IOrderRepository
{
    Task<Order> GetByIdAsync(Guid id, string buyerId);

    Task<List<Order>> GetByBuyerIdAsync(string buyerId);

    Task<Order> AddOrderAsync(Order order);

    Task UpdateOrderStatusAsync(string paymentIntentId, string chargeStatus);
}
