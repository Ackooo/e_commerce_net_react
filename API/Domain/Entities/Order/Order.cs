namespace Domain.Entities.Order;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Domain.Entities.User;
using Domain.Shared.Enums;

[Table(nameof(Order), Schema = "Store")]
public class Order
{
	[Key]
	public Guid Id { get; set; } = Guid.CreateVersion7();

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

	[Range(0, long.MaxValue)]
    public long Subtotal { get; set; }

	[Range(0, long.MaxValue)]
    public long DeliveryFee { get; set; }

    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

    [Required]
    public required Address ShippingAddress { get; set; }


	[Required]
	[MaxLength(256)]
	public required string BuyerId { get; set; }

    public required List<OrderItem> OrderItems { get; set; }

	[MaxLength(256)]
	public string? PaymentIntentId { get; set; }

    public long GetTotal()
    {
        return Subtotal + DeliveryFee;
    }

}
