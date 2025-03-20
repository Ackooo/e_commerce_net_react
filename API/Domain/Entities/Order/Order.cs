namespace Domain.Entities.Order;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Domain.Entities.User;
using Domain.Shared.Enums;

[Table(nameof(Order), Schema = "Store")]
public class Order
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public DateTime OrderDate { get; set; } = DateTime.Now; // UtcNow for some sql
    public long Subtotal { get; set; }
    public long DeliveryFee { get; set; }
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
    [Required]
    public Address ShippingAddress { get; set; }

    public required string BuyerId { get; set; }
    public required List<OrderItem> OrderItems { get; set; }

    public string? PaymentIntentId { get; set; }

    public long GetTotal()
    {
        return Subtotal + DeliveryFee;
    }

}
