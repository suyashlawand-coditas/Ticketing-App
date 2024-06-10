using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.ServiceContracts;

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

    public IActionResult Profile()
    {
        return View();
    }
}
