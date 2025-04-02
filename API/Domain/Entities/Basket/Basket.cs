namespace Domain.Entities.Basket;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(Basket), Schema = "Store")]
public class Basket
{

    [Column]
    [Key]
    [DefaultValue("NEWSEQUENTIALID()")]
    public Guid Id { get; set; } = Guid.CreateVersion7();

    [Column]
    [Required]
    public required Guid UserId { get; set; }

    #region Stripe

    [Column]
    [MaxLength(256)]
    public string? PaymentIntentId { get; set; }

    [Column]
    [MaxLength(256)]
    public string? ClientSecret { get; set; }

    #endregion

    #region NavigationProperies

    public ICollection<BasketItem> BasketItems { get; set; } = [];

    #endregion

}
