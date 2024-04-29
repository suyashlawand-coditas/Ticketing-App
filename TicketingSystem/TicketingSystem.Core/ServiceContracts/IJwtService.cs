using System.Security.Claims;
using TicketingSystem.Core.Domain.Entities;

namespace TicketingSystem.Core.ServiceContracts
{
    public interface IJwtService
    {
        string CreateJwtToken(User user);

        IEnumerable<Claim> VerifyJwtToken(String token);
    }
}
