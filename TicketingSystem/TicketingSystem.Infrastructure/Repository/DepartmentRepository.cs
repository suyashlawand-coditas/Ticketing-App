using Microsoft.EntityFrameworkCore;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Infrastructure.DBContext;
using TicketingSystem.Core.Exceptions;

namespace TicketingSystem.Infrastructure.Repository 
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _dbContext; 

        public DepartmentRepository(ApplicationDbContext dbContext) {_dbContext = dbContext;}

        public async Task<Department> AddDepartment(Department department)
        {
            await _dbContext.AddAsync(department);
            return department;
        }

        public async Task<List<Department>> GetAllDepartments() => await _dbContext.Departments.ToListAsync();

        public async Task<Department> GetDepartmentById(Guid DepartmentId)
        {
            Department? department = await _dbContext.Departments.FirstOrDefaultAsync(
                (dept) => dept.DepartmentId == DepartmentId
            );
            if (department == null) throw new EntityNotFoundException<Department>();
            return department;
        }

        public async Task<Department> UpdateDepartment(Department department)
        {
            Department? departmentToUpdate = await _dbContext.Departments.FirstOrDefaultAsync(
                (dept) => dept.DepartmentId == department.DepartmentId
            );

            if (departmentToUpdate == null) throw new EntityNotFoundException<Department>();
            departmentToUpdate.Name = department.Name;

            await _dbContext.SaveChangesAsync();
            return department;
        }
    }
}
