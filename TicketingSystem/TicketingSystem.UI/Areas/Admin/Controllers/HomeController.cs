using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.Enums;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.UI.Areas.Admin.Attributes;

namespace TicketingSystem.UI.Areas.Admin.Controllers;

[Area("Admin")]
public class HomeController : Controller
{

    private readonly IUserService _userService;

    public HomeController(IUserService userService) 
    { 
        _userService = userService;
    }

    public IActionResult Index()
    {
        return LocalRedirect("/Admin/TicketManagement/AssignedTickets");
    }

    public async Task<IActionResult> Profile()
    {
        return View();
    }

    public IActionResult ProfileSettings() { 
        return View();
    }

}
