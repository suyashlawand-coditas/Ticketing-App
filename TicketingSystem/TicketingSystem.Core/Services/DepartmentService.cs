using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.ServiceContracts;
namespace TicketingSystem.Core.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<List<Department>> GetAllDepartments()
    {
        return await _departmentRepository.GetAllDepartments();
    }

    public async Task<Department> GetDepartmentById(Guid departmentId)
    {
         return await _departmentRepository.GetDepartmentById(departmentId);
    }
}
