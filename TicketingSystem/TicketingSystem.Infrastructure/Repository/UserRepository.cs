using Microsoft.EntityFrameworkCore;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.Enums;
using TicketingSystem.Core.Exceptions;
using TicketingSystem.Infrastructure.DBContext;

namespace TicketingSystem.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CheckDuplicateEmail(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> CheckDuplicatePhone(string phone)
        {
            return await _dbContext.Users.AnyAsync(u => u.Phone == phone);
        }

        public async Task<User> CreateUser(User user)
        {
            try
            {
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return user;
            } catch (DbUpdateException)
            {
                throw new UniqueConstraintFailedExeption("Unable to create new user");
            }
        }

        public async Task<User> DeactivateUser(Guid userId)
        {
            User? targetUser = await _dbContext.Users.FirstOrDefaultAsync(user => user.UserId == userId);
            if (targetUser == null) throw new EntityNotFoundException(nameof(User));
            return targetUser;
        }

        public async Task<User?> FindUserByEmailId(string email)
        {
            return await _dbContext.Users
                .Include(user => user.Department)
                .Include(user => user.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        } 

        public async Task<User?> FindUserByUserId(Guid userId)
        {
            return await _dbContext.Users
                .Include(user => user.UserCreation)
                .Include(user => user.UserCreation!.CreatorUser)
                .Include(user => user.UserCreation!.CreatorUser.Department)
                .Include(user => user.Department)
                .Include(user => user.Role)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User?> GetAdminWithLeastTickets(Guid departmentId)
        {
            return await _dbContext.Users
                .Include(user => user.TicketAssignments)
                .Where(user => user.DepartmentId == departmentId && user.Role.Role == Role.Admin)
                .OrderBy(u => u.TicketAssignments.Count)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetUserCount(string? name)
        {
            return String.IsNullOrEmpty(name) ? await _dbContext.Users.CountAsync()
                : await _dbContext.Users.Where(user => user.FullName.Contains(name)).CountAsync();
        }

        public async Task<List<User>> GetUsersList(int page, int limit, string? search)
        {
            if (String.IsNullOrEmpty(search))
            {
                return await _dbContext.Users
                    .Include(user => user.Department)
                    .Include(user => user.Role)
                    .Skip(limit * (page - 1))
                    .Take(limit)
                    .ToListAsync();
            }
            else
            {
                return await _dbContext.Users
                    .Include(user => user.Department)
                    .Include(user => user.Role)
                    .Where(user => user.FullName.Contains(search))
                    .Skip(limit * (page - 1))
                    .Take(limit)
                    .ToListAsync();
            }
        }

        public async Task<User> UpdateUser(User user)
        {
            User? selectedUser = await _dbContext.Users.FirstOrDefaultAsync(usr => usr.UserId == user.UserId);
            if (selectedUser == null) throw new EntityNotFoundException(nameof(User));

            user.UserId = selectedUser.UserId;
            selectedUser = user;

            await _dbContext.SaveChangesAsync();
            return user;
        }
    }
}
