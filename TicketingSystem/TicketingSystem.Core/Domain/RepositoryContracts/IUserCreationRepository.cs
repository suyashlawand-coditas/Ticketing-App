using TicketingSystem.Core.Domain.Entities;

namespace TicketingSystem.Core.Domain.RepositoryContracts
{
    public interface IUserCreationRepository
    {
        Task<UserCreation> CreateUserCreationEntry(Guid createdByUserId, Guid newUserId);
    }
}
