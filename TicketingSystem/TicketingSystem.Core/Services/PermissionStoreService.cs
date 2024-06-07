using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.Enums;
using TicketingSystem.Core.ServiceContracts;

namespace TicketingSystem.Core.Services
{
    public sealed class PermissionStoreService : IPermissionStoreService
    {
        private readonly List<PermissionCacheDto> _permissionCacheStore = [];

        private static PermissionStoreService? _service;
        private PermissionStoreService() { }

        public static PermissionStoreService Initialize() {
            if (_service == null)
            {
                _service = new PermissionStoreService();
            }
            return _service;
        }

        public void AddPermissionToStore(PermissionCacheDto permissionCache)
        {
            if (!_permissionCacheStore.Contains(permissionCache))
            _permissionCacheStore.Add(permissionCache);
        }

        public PermissionCacheDto? GetPermissionFromStore(Guid userId, Permission permission)
        {
            return _permissionCacheStore.FirstOrDefault(
                    permisionCache => permisionCache.UserId == userId && 
                    permisionCache.Permission == permission ||
                    permisionCache.UserId == userId &&
                    permisionCache.Permission == Permission.MASTER_ACCESS
                );
        }

        public void RevokePermissionFromStore(Guid userId, Permission permission)
        {
            _permissionCacheStore.RemoveAll(
                permissionCache => permissionCache.UserId == userId 
                    && permissionCache.Permission == permission
                );
        }
    }
}
