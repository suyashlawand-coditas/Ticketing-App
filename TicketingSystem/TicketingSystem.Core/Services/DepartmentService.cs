using StackExchange.Redis;
using System.Text.Json;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.ServiceContracts;
namespace TicketingSystem.Core.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IDatabase _redisCache;

    public DepartmentService(IDepartmentRepository departmentRepository, IDatabase redisCache)
    {
        _redisCache = redisCache;
        _departmentRepository = departmentRepository;
    }

    public async Task<Department> CreateDepartment(CreateDepartmentDto department)
    {
        await _redisCache.KeyDeleteAsync("Departments");
        return await _departmentRepository.AddDepartment(department.ToDepartment());
    }

    public async Task<List<Department>> GetAllDepartments()
    {
        List<Department>? result = null;

        if (await _redisCache.KeyExistsAsync("Departments"))
        {
            string? departmentsInJson = await _redisCache.StringGetAsync("Departments");
            if (!String.IsNullOrEmpty(departmentsInJson))
            {
                result = JsonSerializer.Deserialize<List<Department>>(departmentsInJson);
            }
        }
        else if (result == null)
        {
            result = await _departmentRepository.GetDepartmentsWithAtleastOneAdmin();
            await _redisCache.StringSetAsync("Departments", JsonSerializer.Serialize<List<Department>>(result), TimeSpan.FromHours(4));
        }

        return result!;
    }

    public async Task<Department> GetDepartmentById(Guid departmentId)
    {
        return await _departmentRepository.GetDepartmentById(departmentId);
    }

    public async Task<int> GetDepartmentCount()
    {
        return await _departmentRepository.GetDepartmentCount();
    }

    public async Task<List<DepartmentDto>> GetDepartmentWithPeopleCount(int pageNo, int countPerPage, string? searchQuery)
    {
        List<DepartmentDto> result = [];
        List<Department> departmentList = await _departmentRepository.GetDepartmentInPages(pageNo, countPerPage, searchQuery);

        foreach (Department department in departmentList)
        {
            result.Add(
                new DepartmentDto()
                {
                    Department = department,
                    PeopleCount = await _departmentRepository.GetPeopleCountInDepartmentByDepartmentId(department.DepartmentId)
                }
                );
        }

        return result;
    }
}
