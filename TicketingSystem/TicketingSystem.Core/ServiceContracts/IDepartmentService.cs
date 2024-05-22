using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.DTOs;

namespace TicketingSystem.Core.ServiceContracts;

public interface IDepartmentService
{
    Task<Department> GetDepartmentById(Guid departmentId);
    Task<List<Department>> GetAllDepartments();

    Task<int> GetDepartmentCount();

    Task<Department> CreateDepartment(CreateDepartmentDto department);
    Task<List<DepartmentDto>> GetDepartmentWithPeopleCount(int pageNo, int countPerPage, string? searchQuery);
}
