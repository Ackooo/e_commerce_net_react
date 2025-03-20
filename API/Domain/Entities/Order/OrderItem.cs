namespace Domain.Entities.Order;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table(nameof(OrderItem), Schema = "Store")]
public class OrderItem
{
	[Key]
	public Guid Id { get; set; } = Guid.CreateVersion7();

	[Range(0, long.MaxValue)]
    public long Price { get; set; }

	[Range(0, long.MaxValue)]
    public int Quantity { get; set; }

	[Required]
	[ForeignKey(nameof(Product))]
    public long ProductId { get; set; }

	[Required]
	[MaxLength(256)]
	public required string Name { get; set; }

	[MaxLength(512)]
	public string? PictureUrl { get; set; }
}