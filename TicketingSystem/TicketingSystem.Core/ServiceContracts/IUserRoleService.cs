using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Enums;

namespace TicketingSystem.Core.ServiceContracts;

public interface IUserRoleService
{
    Task<UserRole> AddUserRole(User user, Role role);
    Task<UserRole> GetUserRoleByUserId(Guid userId);
}
