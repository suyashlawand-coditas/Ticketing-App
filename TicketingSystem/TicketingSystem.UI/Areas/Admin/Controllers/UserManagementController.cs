using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.UI.Areas.Admin.Models;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.UI.Models;
using TicketingSystem.Core.Exceptions;
using TicketingSystem.UI.Areas.Admin.Attributes;
using TicketingSystem.Core.Enums;
namespace TicketingSystem.UI.Areas.Admin.Controllers;


[Area("Admin")]
public class UserManagementController : Controller
{

    private readonly IUserService _userServices;
    private readonly IDepartmentService _departmentService;
    private readonly IAccessPermissionService _accessPermissionService;

    public UserManagementController(IUserService userServices, IDepartmentService departmentService, IAccessPermissionService accessPermissionService)
    {
        _userServices = userServices;
        _departmentService = departmentService;
        _accessPermissionService = accessPermissionService;
    }

    [HttpGet]
    [AuthorizePermission(Permission.CREATE_USER)]
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
    [AuthorizePermission(Permission.SEE_USERS)]
    public async Task<IActionResult> SeeUserById([FromRoute] Guid userId)
    {
        User? user = await _userServices.FindUserByUserId(userId);
        if (user == null)
            throw new EntityNotFoundException<User>();

        return View(user);
    }

    [HttpGet("Admin/UserManagement/EditByUserId/{userId}")]
    [AuthorizePermission(Permission.UPDATE_USER)]
    public async Task<IActionResult> EditByUserId([FromRoute] Guid userId)
    {
        Guid currentUserId = (Guid) ViewBag.User.UserId;
        User? user = await _userServices.FindUserByUserId(userId);
        if (user == null) 
            throw new EntityNotFoundException<User>();
        
        ViewUserDto viewUserDto = ViewUserDto.FromUser(user);
        List<AccessPermission> currentUserAccessPermissions = await _accessPermissionService.GetAccessPermissionsOfUser(currentUserId);
        
        if (user.Role!.Role == Role.Admin &&
            currentUserAccessPermissions.Where(accPermission => accPermission.Permission == Permission.MASTER_ACCESS).Count() >= 1
            )
        {
            ViewBag.Departments = await _departmentService.GetAllDepartments();
            List<AccessPermission> accessPermissions = await _accessPermissionService.GetAccessPermissionsOfUser(userId);
            List<Permission> permissions = accessPermissions.Select(accessPermissions => accessPermissions.Permission).ToList();
            ViewBag.Permissions = accessPermissions;
            ViewBag.UngrantedPermissions = _accessPermissionService.GetUnGrantedAccessPermissionsOfUser(permissions);
        }
        else
        {
            ViewBag.Departments = await _departmentService.GetDepartmentsWithAtleastOneAdmin();
        }        

        return View(viewUserDto);
    }

    [HttpPost("Admin/UserManagement/EditByUserId/{userId}")]
    [AuthorizePermission(Permission.UPDATE_USER)]
    public async Task<IActionResult> EditByUserId([FromRoute] Guid userId, [FromForm] ViewUserDto editedUser)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
            return View(editedUser);
        }
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
    [AuthorizePermission(Permission.CREATE_USER)]
    public async Task<IActionResult> CreateUser([FromForm] CreateUserDto createUser)
    {
        Guid currentUserId = (Guid) ViewBag.User.UserId;
        CreateUserViewModel createUserViewModel = new CreateUserViewModel()
        {
            Departments = await _departmentService.GetDepartmentsWithAtleastOneAdmin()
        };
        ViewBag.CreateUserData = createUserViewModel;

        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
            return View(createUser);
        }
        else
        {
            User newUser = await _userServices.CreateUser(createUser, currentUserId);
            ViewBag.UserCreationMessage = $"New user {newUser.FullName} ({newUser.Email}) was created successfully.";
        }

        return View();
    }

    [AuthorizePermission(Permission.SEE_USERS)]
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
