using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Enums;

namespace TicketingSystem.Core.Domain.RepositoryContracts;

public interface IAccessPermissionRepository {
    Task<AccessPermission> CreateAccessPermissionForUser(Guid userId, Guid grantedByUserId, Permission permission);
    Task<bool> DeleteAccessPermissionById(Guid permissionId);
    Task<List<AccessPermission>> GetAccessPermissionsOfUser(Guid userId);
}
