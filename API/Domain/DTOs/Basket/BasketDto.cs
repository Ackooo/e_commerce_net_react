namespace Domain.DTOs.Basket;

public class BasketDto
{
    public Guid Id { get; set; }
    public string BuyerId { get; set; }
    public List<BasketItemDto> Items { get; set; }

    public string PaymentItentnId { get; set; }
    public string ClientSecret { get; set; }
}
