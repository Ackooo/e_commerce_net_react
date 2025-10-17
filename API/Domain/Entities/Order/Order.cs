namespace Domain.Entities.Order;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.User;
using Domain.Shared.Attributes;
using Domain.Shared.Constants;
using Domain.Shared.Enums;

[Table(nameof(Order), Schema = DbConstants.DbSchemaNameStore)]
public class Order
{

	[Column]
	[Key]
	[DefaultValue("NEWSEQUENTIALID()")]
    [Export(ExportAllowed.No)]
    public Guid Id { get; set; } = Guid.CreateVersion7();

	[Column(TypeName = "datetime2(7)")]
	[Required]
    [Export(ExportAllowed.Yes)]
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

	[Column]
	[Required]
	[Range(0, long.MaxValue)]
    [Export(ExportAllowed.Yes)]
    public required long Subtotal { get; set; }

	[Column]
	[Required]
	[Range(0, long.MaxValue)]
    [Export(ExportAllowed.Yes)]
    public required long DeliveryFee { get; set; }

	[Column]
	[Required]
	[Export(ExportAllowed.Yes)]
	public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

	[Column]
	[Required]
	[ForeignKey(nameof(Address))]
    [Export(ExportAllowed.No)]
    public long ShippingAddressId { get; set; }

	[Column]
	[Required]
	[MaxLength(256)]
	[ForeignKey(nameof(User))]
    [Export(ExportAllowed.No)]
    public required Guid UserId { get; set; }

	[Column]
	[MaxLength(256)]
    [Export(ExportAllowed.No)]
    public string? PaymentIntentId { get; set; }

    #region NavigationProperies

#pragma warning disable CS8618

    [Export(ExportAllowed.No)]
    public required Address ShippingAddress { get; set; }

    [Export(ExportAllowed.No)]
    public User User { get; set; }

    [Export(ExportAllowed.No)]
    public required List<OrderItem> OrderItems { get; set; }

#pragma warning restore CS8618

    #endregion

    #region Methods

    public long GetTotal()
	{
		return Subtotal + DeliveryFee;
	}

	#endregion

}
