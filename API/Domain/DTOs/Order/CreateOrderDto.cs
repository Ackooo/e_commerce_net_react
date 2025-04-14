namespace Domain.DTOs.Order;

using Domain.Entities.Order;
using Domain.Entities.User;

public class CreateOrderDto
{
    public required bool SaveAddress { get; set; }
    public required Address ShippingAddress { get; set; }
}
