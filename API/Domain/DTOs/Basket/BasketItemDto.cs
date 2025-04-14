namespace Domain.DTOs.Basket;

public class BasketItemDto
{
    public required long ProductId { get; set; }
    public required string Name { get; set; }
    public required long Price { get; set; }
    public string? PictureUrl { get; set; }
    public required string Brand { get; set; }
    public required string Type { get; set; }
    public required int Quantity { get; set; }
}
