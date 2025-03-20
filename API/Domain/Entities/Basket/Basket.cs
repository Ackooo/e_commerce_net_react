namespace Domain.Entities.Basket;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(Basket), Schema = "Store")]
public class Basket
{
	//[DatabaseGenerated(DatabaseGeneratedOption.Identity)]

	[Key]
	public Guid Id { get; set; } = Guid.CreateVersion7();

	[Required]
	[MaxLength(256)]
	public required string BuyerId { get; set; }

	#region Stripe

	[MaxLength(256)]
	public string? PaymentIntentId { get; set; }

	[MaxLength(256)]
	public string? ClientSecret { get; set; }

	#endregion

	#region NavigationProperies

	public ICollection<BasketItem> BasketItems { get; set; } = [];

	#endregion

}
