using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.Enums;
using TicketingSystem.Core.ServiceContracts;

namespace TicketingSystem.Core.Services;

public class UserRoleService : IUserRoleService
{
    private readonly IUserRoleRepository _userRoleRepository;

    public UserRoleService(IUserRoleRepository userRoleRepository) { 
        _userRoleRepository = userRoleRepository;
    }

    public Task<UserRole> AddUserRole(User user, Role role)
    {
        return _userRoleRepository.AddRole(user, role); 
    }
}
