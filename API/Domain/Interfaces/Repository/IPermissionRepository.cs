namespace Domain.Interfaces.Repository;

public interface IPermissionRepository
{
	Task<HashSet<string>> GetPermissionsAsync(Guid memberId);
}
