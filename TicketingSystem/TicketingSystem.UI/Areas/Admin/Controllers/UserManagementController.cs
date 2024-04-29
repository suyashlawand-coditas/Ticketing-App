using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.UI.Areas.Admin.Models;
using TicketingSystem.Core.Domain.Entities;
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

    [HttpGet]
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
    public async Task<IActionResult> CreateUser([FromForm] CreateUserDto createUser)
    {
        CreateUserViewModel createUserViewModel = new CreateUserViewModel() { 
            Departments = await _departmentService.GetAllDepartments()
        };
        ViewBag.CreateUserData = createUserViewModel;

        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
            return View(createUser);
        }
        else {
            User newUser = await _userServices.CreateUser(createUser);
            ViewBag.UserCreationMessage = $"New user {newUser.FullName} ({newUser.Email}) was created successfully.";
        }

        return View();
    }

    public IActionResult SeeUsers() 
    {
        return View();
    }

}
