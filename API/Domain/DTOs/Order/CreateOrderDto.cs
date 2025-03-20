namespace Domain.DTOs.Order;

using Domain.Entities.Order;
using Domain.Entities.User;

public class CreateOrderDto
{
    public bool SaveAddress { get; set; }
    public Address ShippingAddress { get; set; }
}
