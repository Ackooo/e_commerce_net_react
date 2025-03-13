namespace Domain.Entities.Order;

using System.ComponentModel.DataAnnotations;

using Domain.Shared.Enums;

public class Order
{
    public int Id { get; set; }
    public required string BuyerId { get; set; }
    [Required]
    public ShippingAddress ShippingAddress { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now; // UtcNow for some sql
    public required List<OrderItem> OrderItems { get; set; }
    public long Subtotal { get; set; }
    public long DeliveryFee { get; set; }
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

    public required string PaymentIntentId { get; set; }

    public long GetTotal()
    {
        return Subtotal + DeliveryFee;
    }

}
