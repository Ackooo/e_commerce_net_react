namespace Domain.Interfaces.Repository;

using Domain.DTOs.Order;

public interface IUserRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="orderDto"></param>
    /// <returns></returns>
    Task<bool> AddUsersAddressAsync(Guid userId, CreateOrderDto orderDto);
}
