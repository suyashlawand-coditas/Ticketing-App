using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Core.Domain.Entities;

namespace TicketingSystem.Core.ServiceContracts;

public interface IDepartmentService
{
    Task<Department> GetDepartmentById(Guid departmentId);
    Task<List<Department>> GetAllDepartments();
}
