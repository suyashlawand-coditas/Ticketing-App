using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.UI.Areas.Admin.Models;
namespace TicketingSystem.UI.Areas.Admin.Controllers;


[Area("Admin")]
public class UserManagementController : Controller
{

    private readonly IUserServices _userServices;
    private readonly IDepartmentService _departmentService;

    public UserManagementController(IUserServices userServices, IDepartmentService departmentService)
    {
        _userServices = userServices;
        _departmentService = departmentService;
    }

    public async Task<IActionResult> CreateUser()
    {
        CreateUserViewModel createUserViewModel = new CreateUserViewModel()
        {
            Departments = await _departmentService.GetAllDepartments(),
        };
        
        ViewBag.CreateUserData = createUserViewModel;
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> InsertUser([FromForm] CreateUserDto createUser)
    {
        if (!ModelState.IsValid)
        {
            return Json(ModelState.ErrorCount);
        }
        else {
            await _userServices.CreateUser(createUser);
        }

        return Json(createUser);
    }

    public IActionResult SeeUsers() 
    {
        return View();
    }

}
