using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Enums;

namespace TicketingSystem.Core.ServiceContracts;

public interface IUserRoleService
{
    Task<UserRole> AddUserRole(User user, Role role);
}
