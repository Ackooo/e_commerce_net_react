namespace Domain.DTOs.Order;

using Domain.Entities.Order;
using Domain.Entities.User;

public class OrderDto
{
    public Guid Id { get; set; }
    public string BuyerId { get; set; }
    public Address ShippingAddress { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
    public long Subtotal { get; set; }
    public long DeliveryFee { get; set; }
    public string OrderStatus { get; set; }
    public long Total { get; set; }

}
