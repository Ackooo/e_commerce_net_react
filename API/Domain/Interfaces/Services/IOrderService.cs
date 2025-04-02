namespace Domain.Interfaces.Services;

using Domain.DTOs.Order;

public interface IOrderService
{
    /// <summary>
    /// Get order from database
    /// </summary>
    /// <param name="id">Order id</param>
    /// <param name="userId">User id</param>
    /// <returns>Order, if exists</returns>
    Task<OrderDto> GetByIdAsync(Guid id, Guid userId);

    /// <summary>
    /// Get order from database
    /// </summary>
    /// <param name="buyerId">User Id</param>
    /// <returns>Order, if exists</returns>
    Task<List<OrderDto>> GetByBuyerIdAsync(Guid userId);

    /// <summary>
    /// Creates an order
    /// </summary>
    /// <param name="userId">User id</param>
    /// <param name="orderDto"></param>
    /// <returns>Id of the created order</returns>
    Task<Guid> CreateOrder(Guid userId, CreateOrderDto orderDto);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="paymentIntentId"></param>
    /// <param name="chargeStatus"></param>
    /// <returns></returns>
    Task UpdateOrderStatusAsync(string paymentIntentId, string chargeStatus);

}
