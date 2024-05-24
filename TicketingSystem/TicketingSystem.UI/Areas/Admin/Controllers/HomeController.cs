using Microsoft.AspNetCore.Mvc;
namespace TicketingSystem.UI.Areas.Admin.Controllers;

[Area("Admin")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return LocalRedirect("/Admin/TicketManagement/AssignedTickets");
    }

    public IActionResult Profile()
    {
        return View();
    }

    public IActionResult ProfileSettings() { 
        return View();
    }

}
