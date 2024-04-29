using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Enums;

namespace TicketingSystem.Core.Domain.RepositoryContracts
{
    public interface IUserRoleRepository
    {
        Task<UserRole> AddRole(User user, Role role);

        Task<UserRole> UpdateRole(Guid UserId, Role role);

        Task<UserRole> GetUserRoleById(Guid UserId);
    }
}
