namespace Domain.Entities.Basket;

public class Basket
{
    public int Id { get; set; }
    public string BuyerId { get; set; }
    public List<BasketItem> Items { get; set; } = [];

    #region Stripe

    public string PaymentIntentId { get; set; }
    public string ClientSecret { get; set; }

    #endregion
    
}
