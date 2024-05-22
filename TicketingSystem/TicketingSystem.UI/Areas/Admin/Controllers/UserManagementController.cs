using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.UI.Areas.Admin.Models;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.UI.Models;
namespace TicketingSystem.UI.Areas.Admin.Controllers;


[Area("Admin")]
public class UserManagementController : Controller
{

    private readonly IUserService _userServices;
    private readonly IDepartmentService _departmentService;

    public UserManagementController(IUserService userServices, IDepartmentService departmentService)
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
        CreateUserViewModel createUserViewModel = new CreateUserViewModel()
        {
            Departments = await _departmentService.GetAllDepartments()
        };
        ViewBag.CreateUserData = createUserViewModel;

        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
            return View(createUser);
        }
        else
        {
            User newUser = await _userServices.CreateUser(createUser);
            ViewBag.UserCreationMessage = $"New user {newUser.FullName} ({newUser.Email}) was created successfully.";
        }

        return View();
    }

    public async Task<IActionResult> SeeUsers([FromQuery] string? search, [FromQuery] string page = "1", [FromQuery] string limit = "10")
    {
        int limitPerPage = 10;
        int currentPage = 1;


        int.TryParse(limit, out limitPerPage);
        int.TryParse(page, out currentPage);
        currentPage = currentPage < 1 ? 1 : currentPage;

        List<User> usersToDisplay = await _userServices.GetUsersList(currentPage, limitPerPage, search);
        int totalUsersToDisplay = await _userServices.GetUserCount(search);
        int totalPages = (int) Math.Ceiling((decimal)totalUsersToDisplay / limitPerPage);
        
        if (currentPage > totalPages)
        {
            return new LocalRedirectResult($"/Admin/UserManagement/SeeUsers?page={totalPages}");
        }
        else
        {
            return View(
                new PagedViewModel<List<ViewUserDto>>()
                {
                    CurrentPage = currentPage,
                    TotalPages = totalPages,
                    PageSize = limitPerPage,
                    ViewModel = usersToDisplay.Select(user => ViewUserDto.FromUser(user)).ToList()
                }
            );
        }
    }
}
