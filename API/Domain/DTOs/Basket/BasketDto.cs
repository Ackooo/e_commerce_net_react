namespace Domain.DTOs.Basket;

public class BasketDto
{
    public required Guid Id { get; set; }    
    public required List<BasketItemDto> Items { get; set; }

    public string? PaymentItentnId { get; set; }
    public string? ClientSecret { get; set; }
}
