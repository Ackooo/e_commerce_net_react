namespace Domain.Entities.Basket;

using System.ComponentModel.DataAnnotations.Schema;

using Product;

[Table(nameof(BasketItem), Schema = "Store")]
public class BasketItem
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public long CIId { get; set; }

    public int Quantity { get; set; }

    #region NavigationProperies

    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    //[ForeignKey("Basket")]
    public Guid BasketId { get; set; }
    public Basket Basket { get; set; }

    #endregion

}
