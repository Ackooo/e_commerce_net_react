namespace Domain.Entities.Basket;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Product;

[Table(nameof(BasketItem), Schema = "Store")]
public class BasketItem
{
	[Key]
	public Guid Id { get; set; } = Guid.CreateVersion7();

	[Range(0, int.MaxValue)]
	public int Quantity { get; set; }

	[ForeignKey(nameof(Basket))]
	public Guid BasketId { get; set; }

	[ForeignKey(nameof(Product))]
	public long ProductId { get; set; }


	#region NavigationProperies

	public Product Product { get; set; }

    public Basket Basket { get; set; }

    #endregion

}
