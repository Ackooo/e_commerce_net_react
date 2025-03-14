namespace Domain.Interfaces.Services;

using Domain.DTOs.Order;

public interface IOrderService
{
    Task<OrderDto> GetByIdAsync(int id, string buyerId);

    Task<List<OrderDto>> GetByBuyerIdAsync(string buyerId);

    Task<int> CreateOrder(string buyerId, CreateOrderDto orderDto);

    Task UpdateOrderStatusAsync(string paymentIntentId, string chargeStatus);

}
