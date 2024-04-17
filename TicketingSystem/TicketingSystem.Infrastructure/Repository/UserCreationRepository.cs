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

    public async Task<UserCreation> CreateUserCreationEntry(User CreatedBy, User NewUser)
    {

        var newUserCreation = new UserCreation()
        {
            CreatedUser = NewUser,
            CreatorUser = CreatedBy,
            CreatedAt = DateTime.Now,
            CreatedUserId = NewUser.UserId,
            CreatorUserId = CreatedBy.UserId,
        };

        await _dbContext.AddAsync(newUserCreation);

        await _dbContext.SaveChangesAsync();

        return newUserCreation;
    }
}
