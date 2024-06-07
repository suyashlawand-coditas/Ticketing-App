using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.Enums;
using TicketingSystem.Core.ServiceContracts;

namespace TicketingSystem.Core.Services
{
    public class AccessPermissionService : IAccessPermissionService
    {

        private readonly IAccessPermissionRepository _accessPermissionRepository;
        public AccessPermissionService(IAccessPermissionRepository accessPermissionRepository)
        {
            _accessPermissionRepository = accessPermissionRepository;
        }

        public async Task<AccessPermission> CreateAccessPermissionForUser(Guid userId, Guid grantedByUserId, Permission permission)
        {
            return await _accessPermissionRepository.CreateAccessPermissionForUser(userId, grantedByUserId, permission);
        }

        public async Task<bool> DeleteAccessPermissionById(Guid permissionId)
        {
            return await _accessPermissionRepository.DeleteAccessPermissionById(permissionId);
        }

        public async Task<List<AccessPermission>> GetAccessPermissionsOfUser(Guid userId)
        {
            return await _accessPermissionRepository.GetAccessPermissionsOfUser(userId);
        }

        public List<Permission> GetUnGrantedAccessPermissionsOfUser(List<Permission> accessPermissions)
        {
            List<Permission> allPermissions = Enum.GetValues(typeof(Permission)).Cast<Permission>().ToList();
            allPermissions.Remove(Permission.MASTER_ACCESS);
            return allPermissions.Where(permission => !accessPermissions.Contains(permission)).ToList();
        }
    }
}
