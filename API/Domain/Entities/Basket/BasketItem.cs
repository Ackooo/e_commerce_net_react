namespace Domain.Entities.Basket;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Product;

[Table(nameof(BasketItem), Schema = "Store")]
public class BasketItem
{

	[Column]
	[Key]
	[DefaultValue("NEWSEQUENTIALID()")]
	public Guid Id { get; set; } = Guid.CreateVersion7();

	[Column]
	[Required]
	[Range(0, int.MaxValue)]
	[DefaultValue(1)]
	public int Quantity { get; set; }

	[Column]
	[Required]
	[ForeignKey(nameof(Basket))]
	public Guid BasketId { get; set; }

	[Column]
	[Required]
	[ForeignKey(nameof(Product))]
	public long ProductId { get; set; }


	#region NavigationProperies

	public Product Product { get; set; }

    public Basket Basket { get; set; }

    #endregion

}
