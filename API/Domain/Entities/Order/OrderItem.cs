namespace Domain.Entities.Order;

using System.ComponentModel.DataAnnotations.Schema;


[Table(nameof(OrderItem), Schema = "Store")]
public class OrderItem
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public long Price { get; set; }
    public int Quantity { get; set; }
    
    public long ProductId { get; set; }
    public string Name { get; set; }
    public string? PictureUrl { get; set; }
}