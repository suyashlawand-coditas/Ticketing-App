using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Core.Domain.Entities;

namespace TicketingSystem.Core.Domain.RepositoryContracts
{
    public interface IDepartmentRepository
    {
        Task<Department> AddDepartment(Department department);
        Task<Department> UpdateDepartment(Department department);
        Task<List<Department>> GetAllDepartments();
        Task<Department> GetDepartmentById(Guid DepartmentId);
    }
}
