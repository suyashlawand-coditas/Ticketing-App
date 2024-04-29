using Microsoft.EntityFrameworkCore;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.Enums;
using TicketingSystem.Infrastructure.DBContext;

namespace TicketingSystem.Infrastructure.Repository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRoleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserRole> AddRole(User user, Role role)
        {
            var userRole = new UserRole()
            {
                Role = role,
                User = user,
                CreatedAt = DateTime.Now
            };

            await _dbContext.UserRoles.AddAsync(userRole);
            await _dbContext.SaveChangesAsync();
            return userRole;
        }

        public async Task<UserRole> GetUserRoleById(Guid UserId)
        {
            UserRole userRole = await _dbContext.UserRoles.FirstAsync(
                    usrRole => usrRole.UserId == UserId
                );
            return userRole;
        }

        public Task<UserRole> UpdateRole(Guid UserId, Role role)
        {
            throw new NotImplementedException();
        }
    }
}
