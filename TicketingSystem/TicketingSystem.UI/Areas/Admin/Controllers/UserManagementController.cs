using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.UI.Areas.Admin.Models;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.UI.Models;
using TicketingSystem.Core.Exceptions;
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

    [HttpGet("Admin/UserManagement/SeeUsers/{userId}")]
    public async Task<IActionResult> SeeUserById([FromRoute] Guid userId)
    {
        User? user = await _userServices.FindUserByUserId(userId);
        if (user == null)
            throw new EntityNotFoundException<User>();
        

        return View(user);
    }

    [HttpGet("Admin/UserManagement/EditByUserId/{userId}")]
    public async Task<IActionResult> EditByUserId([FromRoute] Guid userId)
    {
        User? user = await _userServices.FindUserByUserId(userId);
        if (user == null)
            throw new EntityNotFoundException<User>();
        ViewUserDto viewUserDto = ViewUserDto.FromUser(user);
        ViewBag.Departments = await _departmentService.GetAllDepartments();

        return View(viewUserDto);
    }

    [HttpPost("Admin/UserManagement/EditByUserId/{userId}")]
    public async Task<IActionResult> EditByUserId([FromRoute] Guid userId, [FromForm] ViewUserDto editedUser)
    {
        User? user = await _userServices.FindUserByUserId(userId);
        if (user == null)
            throw new EntityNotFoundException<User>();

        user.FullName = editedUser.FullName;
        user.Email = editedUser.Email;
        user.Phone = editedUser.Phone;
        user.DepartmentId = editedUser.DepartmentId;
        user.IsActive = editedUser.IsActive;

        await _userServices.UpdateUser(user);

        return LocalRedirect($"/Admin/UserManagement/SeeUsers/{userId}");
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

    [HttpGet]
    public IActionResult ChangePassword() 
    {
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
