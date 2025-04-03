namespace Domain.Interfaces.Repository;

using Domain.DTOs.Order;
using Domain.Entities.User;

public interface IUserRepository
{
    #region User

    /// <summary>
    /// Get user with all their roles and permissions
    /// </summary>
    /// <param name="id"></param>
    /// <returns>User if exists, otherwise null</returns>
    Task<User?> GetUserWithPermissionsAsync(Guid id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="orderDto"></param>
    /// <returns></returns>
    Task<bool> AddUsersAddressAsync(Guid userId, CreateOrderDto orderDto);

    #endregion

    #region Role

    /// <summary>
    /// Get role by name
    /// </summary>
    /// <param name="name"></param>
    /// <returns>Role if exists, otherwise exception</returns>
    Task<Role> GetRoleAsync(string name);

    /// <summary>
    /// Get all available roles
    /// </summary>
    /// <returns></returns>
    Task<List<Role>> GetAvailableRolesAsync();

    /// <summary>
    /// Checks if the user has the role
    /// </summary>
    /// <param name="userId">User id </param>
    /// <param name="roleId">Role id</param>
    Task<bool> CheckUserRoleExistenceAsync(Guid userId, Guid roleId);

    /// <summary>
    /// Add user to a role
    /// </summary>
    /// <param name="userId">User id</param>
    /// <param name="roleId">Role id</param>
    /// <returns>True if </returns>
    Task<bool> AddUserRoleAsync(Guid userId, Guid roleId);

    #endregion

    #region Permission

    /// <summary>
    /// 
    /// </summary>
    /// <param name="memberId"></param>
    /// <returns></returns>
    Task<HashSet<string>> GetPermissionsAsync(Guid memberId);

    #endregion

}
