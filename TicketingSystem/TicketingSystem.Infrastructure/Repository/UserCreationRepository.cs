using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Infrastructure.DBContext;

namespace TicketingSystem.Infrastructure.Repository;

public class UserCreationRepository : IUserCreationRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserCreationRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserCreation> CreateUserCreationEntry(Guid createdByUserId, Guid newUserId)
    {

        var newUserCreation = new UserCreation()
        {
            UserCreationId = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            CreatedUserId = newUserId,
            CreatorUserId = createdByUserId,
        };

        await _dbContext.UserCreations.AddAsync(newUserCreation);
        await _dbContext.SaveChangesAsync();

        return newUserCreation;
    }
}
