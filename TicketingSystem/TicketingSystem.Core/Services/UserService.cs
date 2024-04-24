using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.Helpers;
using TicketingSystem.Core.ServiceContracts;

namespace TicketingSystem.Core.Services;

public class UserService : IUserServices
{
    private readonly IUserRepository _userRepository;
    private readonly IUserRoleRepository _roleRepository;
    private readonly IDepartmentRepository _departmentRepository;

    public UserService(IUserRepository userRepository, IUserRoleRepository roleRepository, IDepartmentRepository departmentRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _departmentRepository = departmentRepository;
    }

    public async Task<User> CreateUser(CreateUserDto createUser)
    {
        // TODO: Add Created by user after Implementing Auth Feature Completely.

        ValidationHelper.Validate(createUser);
        User user = createUser.ToUser();
        user.Department = await _departmentRepository.GetDepartmentById(createUser.DepartmentID);        

        await _userRepository.CreateUser(user);
        await _roleRepository.AddRole(user, createUser.Role);
        return user;
    }
}
