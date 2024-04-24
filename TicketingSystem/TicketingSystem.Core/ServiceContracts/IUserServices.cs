using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.DTOs;

namespace TicketingSystem.Core.ServiceContracts;

public interface IUserServices
{
    public Task<User> CreateUser(CreateUserDto createUserDto);
}
