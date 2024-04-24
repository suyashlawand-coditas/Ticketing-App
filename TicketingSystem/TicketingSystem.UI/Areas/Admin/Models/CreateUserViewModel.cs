using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Enums;
namespace TicketingSystem.UI.Areas.Admin.Models;

public class CreateUserViewModel
{
    public List<Role> Roles { get; set; } = new List<Role>() { Role.User, Role.Admin };
    
    public List<Department> Departments { get; set; } = new List<Department>();
}
