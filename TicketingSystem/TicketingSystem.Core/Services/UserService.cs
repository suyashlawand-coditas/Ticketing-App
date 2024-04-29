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
    private readonly ICryptoService _cryptoService;

    public UserService(IUserRepository userRepository, IUserRoleRepository roleRepository, IDepartmentRepository departmentRepository, 
            ICryptoService cryptoService
        )
    {
        _userRepository = userRepository;
        _cryptoService = cryptoService;
        _roleRepository = roleRepository;
        _departmentRepository = departmentRepository;
    }

    public async Task<User> CreateUser(CreateUserDto createUser)
    {
        string salt;
        ValidationHelper.Validate(createUser);
        User user = createUser.ToUser();
        user.PasswordHash = _cryptoService.Encrypt(createUser.Password, out salt);
        user.PasswordSalt = salt;
        user.Department = await _departmentRepository.GetDepartmentById(createUser.DepartmentID);

        await _userRepository.CreateUser(user);
        await _roleRepository.AddRole(user, createUser.Role);
        return user;
    }

    public async Task<User?> FindUserByEmail(string email) 
    {
        return await _userRepository.FindUserByEmailId(email);
    }
}
