using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.Enums;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.UI.Areas.Admin.Attributes;
using TicketingSystem.UI.Models;
namespace TicketingSystem.UI.Areas.Admin.Controllers;


[Area("Admin")]
public class DepartmentManagementController : Controller
{
    private readonly IDepartmentService _departmentService;

    public DepartmentManagementController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [AuthorizePermission(Permission.SEE_DEPARTMENTS)]
    public async Task<IActionResult> SeeDepartments([FromQuery] string page = "1", [FromQuery] string? search = null)
    {
        int pageLimit = 10;
        int currentPage = 1;
        int departmentCount = await _departmentService.GetDepartmentCount();
        int totalPages = (int)Math.Ceiling((double)departmentCount / pageLimit);

        int.TryParse(page, out currentPage);
        if (currentPage > totalPages)
        {
            return new LocalRedirectResult($"/Admin/DepartmentManagement/SeeDepartments?page={totalPages}");
        }

        List<DepartmentDto> departments = await _departmentService.GetDepartmentWithPeopleCount(currentPage, pageLimit, search);
        PagedViewModel<List<DepartmentDto>> pagedViewModel = new()
        {
            CurrentPage = currentPage,
            PageSize = pageLimit,
            ViewModel = departments,
            TotalPages = totalPages
        };
        return View(pagedViewModel);
    }

    [AuthorizePermission(Permission.CREATE_DEPARTMENT)]
    [HttpGet]
    public IActionResult CreateDepartment()
    {
        return View();
    }

    [AuthorizePermission(Permission.CREATE_DEPARTMENT)]
    [HttpPost]
    public async Task<IActionResult> CreateDepartment([FromForm] CreateDepartmentDto departmentDto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage);
            return View(departmentDto);
        }

        Department department = await _departmentService.CreateDepartment(departmentDto);
        if (department != null)
        {
            ViewBag.Message = $"Added new department {department.Name}";
            return View(departmentDto);
        }
        else
        {
            ViewBag.Error = "Unable to create new Department";
            return View(departmentDto);
        }
    }
}
