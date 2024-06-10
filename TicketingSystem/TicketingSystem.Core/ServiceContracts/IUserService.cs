using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.DTOs;

namespace TicketingSystem.Core.ServiceContracts;

public interface IUserService
{
    public Task<User> CreateUser(CreateUserDto createUserDto, Guid createdByUserId);

    public Task<User?> FindUserByEmail(string email);

    public Task<User?> FindUserByUserId(Guid userId);

    Task<List<User>> GetUsersList(int page, int limit, string? search);

    Task<int> GetUserCount(string? name);

    Task<User?> UpdateUser(User user);
}
