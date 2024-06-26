﻿using TicketingSystem.Core.Domain.Entities;

namespace TicketingSystem.Core.Domain.RepositoryContracts
{
    public interface IUserRepository
    {
        Task<User> CreateUser(User user);

        Task<User> UpdateUser(User user);

        Task<User> DeactivateUser(Guid userId);

        Task<User?> FindUserByEmailId(string email);

        Task<User?> FindUserByUserId(Guid userId);

        Task<List<User>> GetUsersList(int page, int limit, string? search);

        Task<User?> GetAdminWithLeastTickets(Guid departmentId);

        Task<int> GetUserCount(string? name);

        Task<bool> CheckDuplicateEmail(string email);

        Task<bool> CheckDuplicatePhone(string phone);
    }
}
