using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.DTOs;
namespace TicketingSystem.UI.Controllers;


[Route("[controller]/[action]")]
public class AuthController : Controller
{

    [HttpPost]
    public IActionResult PerformLogin([FromForm] LoginDto loginCredentials)
    {
        return Json(loginCredentials);
    }

    [HttpGet]
    public IActionResult PerformLogout()
    {
        return RedirectToAction("Login", "Auth");
    }

    public IActionResult Login()
    {
        return View();
    }
}
