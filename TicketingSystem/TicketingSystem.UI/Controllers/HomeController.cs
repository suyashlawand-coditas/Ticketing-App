using Microsoft.AspNetCore.Mvc;
namespace TicketingSystem.UI.Controllers;


[Route("[controller]/[action]")]
public class HomeController : Controller
{
    [Route("/")]
    public IActionResult Index()
    {
        return View();
    }
}
