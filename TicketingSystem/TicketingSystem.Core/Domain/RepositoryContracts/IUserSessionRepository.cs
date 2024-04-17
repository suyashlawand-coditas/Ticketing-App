using TicketingSystem.Core.Domain.Entities;

namespace TicketingSystem.Core.Domain.RepositoryContracts
{
    public interface IUserSessionRepository
    {
        Task<UserSession> GetUserSessionByUserId(Guid UserId);
        Task<bool> DeleteUserSession(Guid SessionId);
        Task<UserSession> CreateUserSession(UserSession userSession);
    }
}
