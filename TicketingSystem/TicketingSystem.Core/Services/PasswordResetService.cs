using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.Exceptions;
using TicketingSystem.Core.ServiceContracts;

namespace TicketingSystem.Core.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IPasswordResetSessionRepository _passwordResetSessionRepository;
        private readonly ICryptoService _cryptoService;

        public PasswordResetService(IUserService userService, IEmailService emailService, IPasswordResetSessionRepository passwordResetSessionRepository, ICryptoService cryptoService)
        {
            _emailService = emailService;
            _userService = userService;
            _passwordResetSessionRepository = passwordResetSessionRepository;
            _cryptoService = cryptoService;
        }

        public async Task ChangePassword(string passwordToChange, PasswordResetSession passwordResetSession)
        {

            if (passwordResetSession.LinkIsUsed)
            {
                throw new PasswordLinkExpiredException();
            }

            User userToChangePassword = await _userService.FindUserByUserId(
                    passwordResetSession.CreatedForUserId
                ) ?? throw new EntityNotFoundException(nameof(User), passwordResetSession.CreatedForUserId.ToString());

            userToChangePassword.ForceToResetPassword = false;
            await _userService.UpdateUser(userToChangePassword);

            string hash = _cryptoService.Encrypt(passwordToChange, out string salt);
            userToChangePassword.PasswordHash = hash;
            userToChangePassword.PasswordSalt = salt;

            await _userService.UpdateUser(userToChangePassword);
            
            passwordResetSession.LinkIsUsed = true;
            await _passwordResetSessionRepository.UpdatePasswordResetSession(passwordResetSession);
        }

        public async Task<PasswordResetSession> CreateResetSession(PasswordResetSession resetSession, string linkPrefix, bool forceUser)
        {
            PasswordResetSession passwordResetSession = await _passwordResetSessionRepository.CreateAsync(resetSession);
            User userToSendPasswordResetLink = await _userService.FindUserByUserId(passwordResetSession.CreatedForUserId) ?? throw new EntityNotFoundException(nameof(User), passwordResetSession.CreatedForUserId.ToString());

            if (forceUser)
            {
                userToSendPasswordResetLink.ForceToResetPassword = true;
                await _userService.UpdateUser(userToSendPasswordResetLink);
            }

            string pwdResetSessionIdHash = _cryptoService.GenerateSHA256Hash(passwordResetSession.PasswordResetSessionID.ToString());
            string resetLink = $"{linkPrefix}/{pwdResetSessionIdHash}";

            _ = _emailService.SendPasswordResetEmail(userToSendPasswordResetLink.Email, resetLink);

            return passwordResetSession;
        }

        public async Task<PasswordResetSession?> ExpirePasswordResetSession(PasswordResetSession passwordResetSession)
        {
            passwordResetSession.LinkIsUsed = true;
            return await _passwordResetSessionRepository.UpdatePasswordResetSession(passwordResetSession);

        }

        public async Task<PasswordResetSession?> FindPasswordResetSessionByIdHash(string hash)
            => await _passwordResetSessionRepository.FindPasswordResetSessionByIdHash(hash);
    }
}
