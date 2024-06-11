using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.Infrastructure.DBContext;

namespace TicketingSystem.Infrastructure.Repository
{
    public class PasswordResetSessionRepository : IPasswordResetSessionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICryptoService _cryptoService;

        public PasswordResetSessionRepository(ApplicationDbContext dbContext, ICryptoService cryptoService)
        {
            _dbContext = dbContext;
            _cryptoService = cryptoService;
        }

        public async Task<PasswordResetSession> CreateAsync(PasswordResetSession resetSession)
        {
            await _dbContext.PasswordResetSessions.AddAsync(resetSession);
            await _dbContext.SaveChangesAsync();
            return resetSession;
        }

        public async Task<PasswordResetSession?> FindPasswordResetSessionById(Guid passwordResetSessionId)
        {
            return await _dbContext.PasswordResetSessions
                .Include(
                    pwdResetSession => pwdResetSession.CreatedForUser
                )
                .FirstOrDefaultAsync(
                    pwdResetSession => pwdResetSession.PasswordResetSessionID == passwordResetSessionId
                );
        }

        public async Task<PasswordResetSession?> FindPasswordResetSessionByIdHash(string hash)
        {
            List<PasswordResetSession> passwordResetSessions = await _dbContext.PasswordResetSessions
                .Include(pwdRst => pwdRst.CreatedForUser)
                .Where(pwdRst => pwdRst.LinkIsUsed == false)
                .Where(pwdRst => pwdRst.CreatedAt.AddMinutes(30) > DateTime.Now)
                .ToListAsync();

            foreach (var passwordResetSession in passwordResetSessions)
            {
                if (_cryptoService.GenerateSHA256Hash(passwordResetSession.PasswordResetSessionID.ToString()) == hash)
                {
                    return passwordResetSession;
                }
            }
            return null;
        }

        public async Task<PasswordResetSession?> UpdatePasswordResetSession(PasswordResetSession resetSession)
        {
            PasswordResetSession? resetSessionToUpdate = await _dbContext.PasswordResetSessions.FirstOrDefaultAsync(resetSesn => resetSession.PasswordResetSessionID == resetSesn.PasswordResetSessionID);

            if (resetSessionToUpdate != null)
            {
                resetSessionToUpdate = resetSession;
                await _dbContext.SaveChangesAsync();
            }

            return resetSessionToUpdate;
        }
    }
}
