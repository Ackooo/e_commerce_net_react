namespace Domain.Interfaces.Services;

using Domain.Entities.User;
using Domain.Shared.Enums;

public interface IUserService
{
    /// <summary>
    /// Add Role to the user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleName"></param>
    /// <returns>True if operation was successful</returns>
    Task<bool> AddRoleToUserAsync(Guid userId, string roleName);

    /// <summary>
    /// Add permissions to user, if missing
    /// </summary>
    /// <param name="user">User</param>
    /// <param name="permissions">Permissions</param>
    /// <returns></returns>
    Task<bool> AddPermissionsToUser(User user, Permissions[] permissions);
    
}
