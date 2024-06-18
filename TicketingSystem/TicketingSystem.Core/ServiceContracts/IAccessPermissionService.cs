using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Enums;

namespace TicketingSystem.Core.ServiceContracts
{
    public interface IAccessPermissionService
    {
        Task<AccessPermission> CreateAccessPermissionForUser(Guid userId, Guid grantedByUserId, Permission permission);
        Task<bool> DeleteAccessPermissionById(Guid permissionId);
        Task<List<AccessPermission>> GetAccessPermissionsOfUser(Guid userId);
        List<Permission> GetUnGrantedAccessPermissionsOfUser(List<Permission> accessPermissions);
        Task<AccessPermission?> GetAccessPermissionById(Guid permissionId);
    }
}
