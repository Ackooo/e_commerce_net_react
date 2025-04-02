namespace Domain.Interfaces.Repository;

using Domain.Entities.Order;

public interface IOrderRepository
{
    /// <summary>
    /// Get order from database
    /// </summary>
    /// <param name="id">Order id</param>
    /// <param name="buyerId">User id</param>
    /// <returns>Order if exists, otherwise null</returns>
    Task<Order> GetByIdAsync(Guid id, Guid userId, bool isTracked = false);

    /// <summary>
    /// Get order from database
    /// </summary>
    /// <param name="userId">User Id</param>
    /// <returns>Order if exists, otherwise null</returns>
    Task<List<Order>> GetByBuyerIdAsync(Guid userId);

    /// <summary>
    /// Adds order to database
    /// </summary>
    /// <param name="order"></param>
    /// <returns>Created order</returns>
    Task<Order> AddOrderAsync(Order order);

    Task UpdateOrderStatusAsync(string paymentIntentId, string chargeStatus);
}
