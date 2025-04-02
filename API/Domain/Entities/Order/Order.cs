namespace Domain.Entities.Order;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Domain.Entities.User;
using Domain.Shared.Enums;

[Table(nameof(Order), Schema = "Store")]
public class Order
{

	[Column]
	[Key]
	[DefaultValue("NEWSEQUENTIALID()")]
	public Guid Id { get; set; } = Guid.CreateVersion7();

	[Column(TypeName = "datetime2(7)")]
	[Required]
	public DateTime OrderDate { get; set; } = DateTime.UtcNow;

	[Column]
	[Required]
	[Range(0, long.MaxValue)]
	public required long Subtotal { get; set; }

	[Column]
	[Required]
	[Range(0, long.MaxValue)]
	public required long DeliveryFee { get; set; }

	[Column]
	[Required]
	public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

	[Column]
	[Required]
	[ForeignKey(nameof(Address))]
	public long ShippingAddressId { get; set; }

	[Column]
	[Required]
	[MaxLength(256)]
	[ForeignKey(nameof(User))]
	public required Guid UserId { get; set; }

	[Column]
	[MaxLength(256)]
	public string? PaymentIntentId { get; set; }

	#region NavigationProperies

	public required Address ShippingAddress { get; set; }

	public User User { get; set; }

	public required List<OrderItem> OrderItems { get; set; }

	#endregion

	#region Methods

	public long GetTotal()
	{
		return Subtotal + DeliveryFee;
	}

	#endregion

}
