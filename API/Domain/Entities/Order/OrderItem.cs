namespace Domain.Entities.Order;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(OrderItem), Schema = "Store")]
public class OrderItem
{

	[Column]
	[Key]
	[DefaultValue("NEWSEQUENTIALID()")]
	public Guid Id { get; set; } = Guid.CreateVersion7();

	[Column]
	[Required]
	[Range(0, long.MaxValue)]
    public required long Price { get; set; }

	[Column]
	[Required]
	[Range(1, long.MaxValue)]
    public required int Quantity { get; set; }

	[Column]
	[Required]
	[ForeignKey(nameof(Product))]
    public required long ProductId { get; set; }

	[Column]
	[Required]
	[MaxLength(256)]
	public required string Name { get; set; }

	[Column]
	[MaxLength(512)]
	public string? PictureUrl { get; set; }

    #region NavigationProperies

#pragma warning disable CS8618

    public Order Order { get; set; }

#pragma warning restore CS8618

    #endregion
}