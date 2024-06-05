using Microsoft.EntityFrameworkCore;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Infrastructure.DBContext;
using TicketingSystem.Core.Exceptions;
using TicketingSystem.Core.Enums;

namespace TicketingSystem.Infrastructure.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DepartmentRepository(ApplicationDbContext dbContext) { _dbContext = dbContext; }

        public async Task<Department> AddDepartment(Department department)
        {
            await _dbContext.AddAsync(department);
            await _dbContext.SaveChangesAsync();
            return department;
        }

        public async Task<List<Department>> GetDepartmentsWithAtleastOneAdmin()
        {
            return await _dbContext.Departments
                .Include(dept => dept.Users)
                .Where( dept => dept.Users.Where( user => user.Role!.Role == Role.Admin ).Count() > 1 ).ToListAsync();
        }

        public async Task<Department> GetDepartmentById(Guid DepartmentId)
        {
            Department? department = await _dbContext.Departments.FirstOrDefaultAsync(
                (dept) => dept.DepartmentId == DepartmentId
            );
            if (department == null) throw new EntityNotFoundException<Department>();
            return department;
        }

        public async Task<int> GetDepartmentCount()
        {
            return await _dbContext.Departments.CountAsync();
        }

        public async Task<List<Department>> GetDepartmentInPages(int pageNo, int countPerPage = 10, string? searchQuery = null)
        {
            pageNo = pageNo < 1 ? 1 : pageNo;

            if (String.IsNullOrEmpty(searchQuery))
            {
                return await _dbContext.Departments
                    .Skip(countPerPage * (pageNo - 1))
                    .Take(countPerPage).ToListAsync();
            }
            else
            {
                return await _dbContext.Departments
                    .Where(d => d.Name.Contains(searchQuery))
                    .Skip(countPerPage * (pageNo - 1))
                    .Take(countPerPage)
                    .ToListAsync();
            }
        }

        public async Task<int> GetPeopleCountInDepartmentByDepartmentId(Guid departmentId)
        {
            return (
                await _dbContext.Departments.Include(d => d.Users)
                .FirstAsync(d => d.DepartmentId == departmentId)
            ).Users.Count();
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
