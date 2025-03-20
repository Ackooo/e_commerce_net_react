namespace Domain.Entities.Basket;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(Basket), Schema = "Store")]
public class Basket
{
	[Key]
	//[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
 
	public Guid Id { get; set; } = Guid.CreateVersion7();
    
	//[Required]
    public required string BuyerId { get; set; }    

    #region Stripe

    public string? PaymentIntentId { get; set; }
    public string? ClientSecret { get; set; }

	#endregion

	#region NavigationProperies

	public ICollection<BasketItem> BasketItems { get; set; } = [];

	#endregion

}
