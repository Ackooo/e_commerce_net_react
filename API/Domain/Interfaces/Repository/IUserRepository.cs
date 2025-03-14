namespace Domain.Interfaces.Repository;

using Domain.DTOs.Order;

public interface IUserRepository
{
    Task<bool> AddUsersAddressAsync(string buyerId, CreateOrderDto orderDto);
}
