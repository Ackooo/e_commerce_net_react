namespace Infrastructure.Persistence.Repositories;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Domain.Entities.User;
using Domain.Interfaces.Repository;

using Microsoft.EntityFrameworkCore;

public class PermissionRepository(StoreContext storeContext) : IPermissionRepository
{
	public async Task<HashSet<string>> GetPermissionsAsync(Guid memberId)
	{
		var roles = await storeContext.Set<User>()
			.Include(x => x.Roles)
			.ThenInclude(x => x.Permissions)
			.Where(x => x.Id == memberId)
			.Select(x => x.Roles)
			.ToArrayAsync();

		return roles
			.SelectMany(x => x)
			.SelectMany(x => x.Permissions)
			.Select(x => x.Name)
			.ToHashSet();
	}
}
