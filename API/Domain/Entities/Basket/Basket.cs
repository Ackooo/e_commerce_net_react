namespace Domain.Entities.Basket;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(Basket), Schema = "Store")]
public class Basket
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public long CIId { get; set; }

    [Required]
    public string BuyerId { get; set; }
    public List<BasketItem> Items { get; set; } = [];

    #region Stripe

    public string? PaymentIntentId { get; set; }
    public string? ClientSecret { get; set; }

    #endregion
    
}
