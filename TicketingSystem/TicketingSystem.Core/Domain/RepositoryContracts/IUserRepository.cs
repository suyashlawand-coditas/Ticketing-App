using System.Threading.Tasks;
using TicketingSystem.Core.Domain.Entities;

namespace TicketingSystem.Core.Domain.RepositoryContracts
{
    public interface IUserRepository
    {
        Task<User> CreateUser(User user);

        Task<User> UpdateUser(User user);

        Task<User> DeactivateUser(Guid userId);

        Task<User?> FindUserByEmailId(string email);
    }
}
