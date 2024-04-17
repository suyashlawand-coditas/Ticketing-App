using TicketingSystem.Core.Domain.Entities;

namespace TicketingSystem.Core.Domain.RepositoryContracts;

public interface IAccessPermissionRepository {
    Task<AccessPermission> CreateAccessPermissionForUser(Guid UserId, AccessPermission accessPermission);
    Task<bool> DeleteAccessPermissionById(Guid PermissionId);
    Task<List<AccessPermission>> GetAccessPermissionOfUser(Guid UserId);
}
