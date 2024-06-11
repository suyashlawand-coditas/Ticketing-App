using Microsoft.AspNetCore.Mvc;

namespace TicketingSystem.UI.Areas.Person.Controllers
{
    public class PasswordResetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
