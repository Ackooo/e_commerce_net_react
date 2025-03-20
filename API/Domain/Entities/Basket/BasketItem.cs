namespace Domain.Entities.Basket;

using System.ComponentModel.DataAnnotations.Schema;

using Product;

[Table(nameof(BasketItem), Schema = "Store")]
public class BasketItem
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public int Quantity { get; set; }
	public Guid BasketId { get; set; }
	public long ProductId { get; set; }

	#region NavigationProperies

	public Product Product { get; set; }
    public Basket Basket { get; set; }

    #endregion

}
