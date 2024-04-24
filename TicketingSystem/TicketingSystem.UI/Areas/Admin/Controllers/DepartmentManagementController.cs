using Microsoft.AspNetCore.Mvc;
namespace TicketingSystem.UI.Areas.Admin.Controllers;


[Area("Admin")]
public class DepartmentManagementController : Controller
{
    public IActionResult SeeDepartments()
    {
        return View();
    }

    public IActionResult CreateDepartment()
    {
        return View();
    }
}
