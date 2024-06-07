using Microsoft.EntityFrameworkCore;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.Enums;
using TicketingSystem.Infrastructure.DBContext;

namespace TicketingSystem.Infrastructure.Repository
{
    public class AccessPermissionRepository : IAccessPermissionRepository
    {

        private readonly ApplicationDbContext _dbContext;

        public AccessPermissionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AccessPermission> CreateAccessPermissionForUser(Guid userId, Guid grantedByUserId, Permission permission)
        {
            var newAccessPermission = new AccessPermission { UserId = userId, CreatedAt = DateTime.UtcNow, GrantedById = grantedByUserId, Permission = permission };
            _dbContext.AccessPermissions.Add(newAccessPermission);
            await _dbContext.SaveChangesAsync();
            return newAccessPermission;
        }

        public async Task<bool> DeleteAccessPermissionById(Guid permissionId)
        {
            AccessPermission? accessToRemove = await _dbContext.AccessPermissions.FirstOrDefaultAsync(accessPermission => accessPermission.PermissionId == permissionId);
            if (accessToRemove != null)
            {
                _dbContext.AccessPermissions.Remove(accessToRemove);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<AccessPermission>> GetAccessPermissionsOfUser(Guid UserId)
        {
            return await _dbContext.AccessPermissions.Where(
                accessPermission => accessPermission.UserId == UserId
                ).ToListAsync();
        }
    }
}
