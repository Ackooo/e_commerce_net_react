namespace Domain.DTOs.Order;

public class OrderItemDto
{
    public required long ProductId { get; set; }
    public required string Name { get; set; }
    public string? PictureUrl { get; set; }
    public required long Price { get; set; }
    public required int Quantity { get; set; }
}