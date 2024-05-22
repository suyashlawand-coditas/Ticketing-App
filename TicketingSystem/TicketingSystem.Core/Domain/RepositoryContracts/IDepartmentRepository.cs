using TicketingSystem.Core.Domain.Entities;

namespace TicketingSystem.Core.Domain.RepositoryContracts
{
    public interface IDepartmentRepository
    {
        Task<int> GetDepartmentCount();
        Task<Department> AddDepartment(Department department);
        Task<Department> UpdateDepartment(Department department);
        Task<List<Department>> GetDepartmentsWithAtleastOneAdmin();
        Task<Department> GetDepartmentById(Guid DepartmentId);
        Task<List<Department>> GetDepartmentInPages(int pageNo, int countPerPage, string? searchQuery);
        Task<int> GetPeopleCountInDepartmentByDepartmentId(Guid departmentId);
    }
}
