namespace Domain.Interfaces.Repository;

using Domain.DTOs.Order;
using Domain.Entities.User;

public interface IUserRepository
{
    /// <summary>
    /// Get user with all their roles and permissions
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<User?> GetUserWithPermissionsAsync(Guid id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="orderDto"></param>
    /// <returns></returns>
    Task<bool> AddUsersAddressAsync(Guid userId, CreateOrderDto orderDto);    
}
