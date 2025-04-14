namespace Infrastructure.Persistence.Repositories;

using Domain.DTOs.Order;
using Domain.Entities.User;
using Domain.Interfaces.Repository;

using Microsoft.EntityFrameworkCore;

public class UserRepository(StoreContext storeContext) : IUserRepository
{

    #region User

    public async Task<User?> GetUserWithPermissionsAsync(Guid id)
    {
        if(id == Guid.Empty) throw new ArgumentNullException(nameof(id));

        return await storeContext.Users
        .Include(u => u.Roles)
        .ThenInclude(u => u.Permissions)
        .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> AddUsersAddressAsync(Guid userId, CreateOrderDto orderDto)
    {
        ArgumentNullException.ThrowIfNull(orderDto);
        if(userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));

        var user = await storeContext.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(x => x.Id == userId);
        if(user == null) return false;

        var address = new Address
        {
            FullName = orderDto.ShippingAddress.FullName,
            Address1 = orderDto.ShippingAddress.Address1,
            Address2 = orderDto.ShippingAddress.Address2,
            City = orderDto.ShippingAddress.City,
            State = orderDto.ShippingAddress.State,
            Zip = orderDto.ShippingAddress.Zip,
            Country = orderDto.ShippingAddress.Country
        };
        user.Address = address;
        storeContext.Update(user);
        return await storeContext.SaveChangesAsync() != 0;
    }

    #endregion

    #region Role

    public Task<Role> GetRoleAsync(string name)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name);
        return storeContext.Roles.AsNoTracking()
            .Where(x => x.Name == name).FirstAsync();
    }

    public Task<List<Role>> GetAvailableRolesAsync()
    {
        return storeContext.Roles.AsNoTracking().ToListAsync();
    }

    public Task<bool> CheckUserRoleExistenceAsync(Guid userId, Guid roleId)
    {
        if(userId==Guid.Empty) throw new ArgumentNullException(nameof(userId));
        if(roleId==Guid.Empty) throw new ArgumentNullException(nameof(roleId));

        return storeContext.UserRoles.AsNoTracking()
            .AnyAsync(x => x.UserId == userId && x.RoleId == roleId);
    }

    public async Task<bool> AddUserRoleAsync(Guid userId, Guid roleId)
    {
        if(userId==Guid.Empty) throw new ArgumentNullException(nameof(userId));
        if(roleId==Guid.Empty) throw new ArgumentNullException(nameof(roleId));

        //FK checks in database
        var userRole = new UserRole
        {
            UserId = userId,
            RoleId = roleId
        };

        await storeContext.UserRoles.AddAsync(userRole);
        return await storeContext.SaveChangesAsync() != 0;
    }

    #endregion

    #region Permission

    public async Task<HashSet<string>> GetPermissionsAsync(Guid memberId)
    {
        if(memberId==Guid.Empty) throw new ArgumentNullException(nameof(memberId));

        var roles = await storeContext.Set<User>()
            .Include(x => x.Roles)
            .ThenInclude(x => x.Permissions)
            .Where(x => x.Id == memberId)
            .Select(x => x.Roles)
            .ToArrayAsync();

        return [.. roles
            .SelectMany(x => x)
            .SelectMany(x => x.Permissions)
            .Select(x => x.Name)];
    }

    #endregion
}
