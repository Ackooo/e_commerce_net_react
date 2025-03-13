namespace Domain.DTOs.Order;

using Domain.Entities.Order;

public class CreateOrderDto
{
    public bool SaveAddress { get; set; }
    public ShippingAddress ShippingAddress { get; set; }
}
