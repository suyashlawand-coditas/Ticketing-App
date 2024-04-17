using Microsoft.EntityFrameworkCore;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.Exceptions;
using TicketingSystem.Infrastructure.DBContext;

namespace TicketingSystem.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CreateUser(User user)
        {
            await _dbContext.Users.AddAsync(user);
            return user;
        }

        public async Task<User> DeactivateUser(Guid userId)
        {
            User? targetUser = await _dbContext.Users.FirstOrDefaultAsync(user => user.UserId == userId);
            if (targetUser == null) throw new EntityNotFoundException<User>();
            return targetUser;
        }

        public async Task<User> UpdateUser(User user)
        {
            User? selectedUser = await _dbContext.Users.FirstOrDefaultAsync(usr => usr.UserId == user.UserId);

            if (selectedUser == null) throw new EntityNotFoundException<User>();

            user.UserId = selectedUser.UserId;
            selectedUser = user;

            await _dbContext.SaveChangesAsync();
            return user;
        }
    }
}
