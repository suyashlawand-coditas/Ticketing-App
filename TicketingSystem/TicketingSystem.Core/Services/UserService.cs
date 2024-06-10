using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.Helpers;
using TicketingSystem.Core.ServiceContracts;

namespace TicketingSystem.Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserRoleRepository _roleRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ICryptoService _cryptoService;
    private readonly IUserCreationRepository _userCreationRepository;

    public UserService(IUserRepository userRepository, IUserRoleRepository roleRepository, IDepartmentRepository departmentRepository,
            ICryptoService cryptoService,
            IUserCreationRepository userCreationRepository
        )
    {
        _userRepository = userRepository;
        _cryptoService = cryptoService;
        _roleRepository = roleRepository;
        _departmentRepository = departmentRepository;
        _userCreationRepository = userCreationRepository;
    }

    public async Task<User> CreateUser(CreateUserDto createUser, Guid createdByUserId)
    {
        string salt;
        ValidationHelper.Validate(createUser);
        User user = createUser.ToUser();
        user.PasswordHash = _cryptoService.Encrypt(createUser.Password, out salt);
        user.PasswordSalt = salt;
        user.Department = await _departmentRepository.GetDepartmentById(createUser.DepartmentID);

        var newUser = await _userRepository.CreateUser(user);
        await _roleRepository.AddRole(user, createUser.Role);
        await _userCreationRepository.CreateUserCreationEntry(createdByUserId, newUser.UserId);
        
        return user;
    }

    public async Task<User?> UpdateUser(User user)
    {
        return await _userRepository.UpdateUser(user);
    }

    public async Task<User?> FindUserByEmail(string email)
    {
        return await _userRepository.FindUserByEmailId(email);
    }

    public async Task<User?> FindUserByUserId(Guid userId)
    {
        return await _userRepository.FindUserByUserId(userId);
    }

    public async Task<int> GetUserCount(string? name)
    {
        return await _userRepository.GetUserCount(name);
    }

    public async Task<List<User>> GetUsersList(int page, int limit, string? search)
    {
        return await _userRepository.GetUsersList(page, limit, search);
    }
}
