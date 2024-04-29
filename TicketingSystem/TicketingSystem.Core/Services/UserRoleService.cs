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

    public async Task<UserRole> AddUserRole(User user, Role role)
    {
        return await _userRoleRepository.AddRole(user, role); 
    }

    public async Task<UserRole> GetUserRoleByUserId(Guid userId)
    {
        return await _userRoleRepository.GetUserRoleById(userId);
    }
}
