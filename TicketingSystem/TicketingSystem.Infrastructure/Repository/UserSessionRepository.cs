using Microsoft.EntityFrameworkCore;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.Exceptions;
using TicketingSystem.Infrastructure.DBContext;

namespace TicketingSystem.Infrastructure.Repository
{
    public class UserSessionRepository : IUserSessionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserSessionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserSession> CreateUserSession(UserSession userSession)
        {
            await _dbContext.AddAsync(CreateUserSession);
            await _dbContext.SaveChangesAsync();
            return userSession;
        }

        public async Task<bool> DeleteUserSession(Guid sessionId)
        {
            UserSession userSession = await _dbContext.UserSessions.FirstOrDefaultAsync(
                    (sessionToFind) => sessionToFind.UserSessionId == sessionId
                ) ?? throw new EntityNotFoundException<UserSession>();

            _dbContext.UserSessions.Remove(userSession);
            int rowsAffected = await _dbContext.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<UserSession> GetUserSessionByUserId(Guid UserId)
        {
            UserSession userSession = await _dbContext.UserSessions.FirstOrDefaultAsync(
                    (sessionToFind) => sessionToFind.User.UserId == UserId
                ) ?? throw new EntityNotFoundException<UserSession>();

            return userSession;
        }
    }
}
