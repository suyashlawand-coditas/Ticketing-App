using Microsoft.AspNetCore.Mvc;
namespace TicketingSystem.UI.Areas.Admin.Controllers;

[Area("Admin")]
public class TicketManagementController : Controller
{
    public IActionResult AllTickets()
    {
        return View();
    }
}
