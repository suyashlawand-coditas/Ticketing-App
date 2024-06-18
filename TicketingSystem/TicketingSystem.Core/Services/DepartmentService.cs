using System.Text.Json;
using System.Text.Json.Serialization;
using TicketingSystem.Core.Constants;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.ServiceContracts;
namespace TicketingSystem.Core.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ICacheService _cacheService;

    public DepartmentService(IDepartmentRepository departmentRepository, ICacheService cacheService)
    {
        _cacheService = cacheService;
        _departmentRepository = departmentRepository;
    }

    public async Task<Department> CreateDepartment(CreateDepartmentDto department)
    {
        await _cacheService.Delete(CacheKeys.AllDepartments);
        return await _departmentRepository.AddDepartment(department.ToDepartment());
    }

    public async Task<List<Department>> GetDepartmentsWithAtleastOneAdmin()
    {
        List<Department>? result = null;

        if (await _cacheService.DoesExist(CacheKeys.DepartmentsWithAtleastOneAdmin))
        {
            string? departmentsInJson = await _cacheService.Get(CacheKeys.DepartmentsWithAtleastOneAdmin) as string;
            if (!String.IsNullOrEmpty(departmentsInJson))
            {
                result = JsonSerializer.Deserialize<List<Department>>(departmentsInJson);
            }
        }
        
        if (result == null)
        {
            result = await _departmentRepository.GetDepartmentsWithAtleastOneAdmin();
            await _cacheService.Set(CacheKeys.DepartmentsWithAtleastOneAdmin, JsonSerializer.Serialize<List<Department>>(result, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            }), TimeSpan.FromHours(4));
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

    public async Task<List<Department>> GetAllDepartments()
    {
        List<Department>? result = null;

        if (await _cacheService.DoesExist(CacheKeys.AllDepartments))
        {
            string? departmentsInJson = await _cacheService.Get(CacheKeys.AllDepartments) as string;
            if (!String.IsNullOrEmpty(departmentsInJson))
            {
                result = JsonSerializer.Deserialize<List<Department>>(departmentsInJson);
            }
        }
        
        if (result == null)
        {
            result = await _departmentRepository.GetAllDepartments();
            await _cacheService.Set(CacheKeys.AllDepartments, JsonSerializer.Serialize<List<Department>>(result, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            }), TimeSpan.FromHours(4));
        }

        return result!;
    }
}
