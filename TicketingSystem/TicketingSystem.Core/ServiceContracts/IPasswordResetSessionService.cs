using TicketingSystem.Core.Domain.Entities;

namespace TicketingSystem.Core.ServiceContracts
{
    public interface IPasswordResetService
    {
        Task<PasswordResetSession> CreateResetSession(PasswordResetSession resetSession, string linkSuffix, bool forceUser);
        public Task<PasswordResetSession?> FindPasswordResetSessionByIdHash(string hash);
        public Task ChangePassword(string passwordToChange, PasswordResetSession passwordResetSessionId);
    }
}
