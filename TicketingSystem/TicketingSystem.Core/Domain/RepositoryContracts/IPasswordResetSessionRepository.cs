using TicketingSystem.Core.Domain.Entities;

namespace TicketingSystem.Core.Domain.RepositoryContracts
{
    public interface IPasswordResetSessionRepository
    {
        Task<PasswordResetSession> CreateAsync(PasswordResetSession resetSession);
        Task<PasswordResetSession?> FindPasswordResetSessionByIdHash(string hash);
        Task<PasswordResetSession?> FindPasswordResetSessionById(Guid passwordResetSessionId);
        Task<PasswordResetSession?> UpdatePasswordResetSession(PasswordResetSession resetSession);
    }
}
