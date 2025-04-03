namespace Infrastructure.Services;

using Domain.Entities.User;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;
using Domain.Shared.Enums;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<bool> AddRoleToUserAsync(Guid userId, string roleName)
    {
        var role = await userRepository.GetRoleAsync(roleName);
        if(await userRepository.CheckUserRoleExistenceAsync(userId, role.Id))
        {
            return true;
        }

        return await userRepository.AddUserRoleAsync(userId, role.Id);
    }

    //TODO:
    public async Task<bool> AddPermissionsToUser(User user, Permissions[] permissions)
    {
        throw new NotImplementedException();
    }
}
