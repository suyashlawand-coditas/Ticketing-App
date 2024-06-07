using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.Enums;

namespace TicketingSystem.Core.ServiceContracts
{
    public interface IPermissionStoreService
    {
        PermissionCacheDto? GetPermissionFromStore(Guid userId, Permission permission);
        void AddPermissionToStore(PermissionCacheDto permissionCache);

        void RevokePermissionFromStore(Guid userId, Permission permission);
    }
}
