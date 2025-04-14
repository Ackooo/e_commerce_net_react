namespace Domain.DTOs.Order;

using Domain.Entities.User;

public class OrderDto
{
    public required Guid Id { get; set; }
    public required Guid BuyerId { get; set; }
    public required Address ShippingAddress { get; set; }
    public required DateTime OrderDate { get; set; }
    public required List<OrderItemDto> OrderItems { get; set; }
    public required long Subtotal { get; set; }
    public required long DeliveryFee { get; set; }
    public required string OrderStatus { get; set; }
    public required long Total { get; set; }

}
