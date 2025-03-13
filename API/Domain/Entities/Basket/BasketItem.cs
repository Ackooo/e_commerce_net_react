namespace Domain.Entities.Basket;

using System.ComponentModel.DataAnnotations.Schema;

using Product;

[Table("BasketItems")]
public class BasketItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }

    #region NavigationProperies

    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int BasketId { get; set; }
    public Basket Basket { get; set; }

    #endregion

}
