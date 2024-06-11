using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.ServiceContracts;

namespace TicketingSystem.UI.Areas.Admin.Controllers;

[Area("Admin")]
public class HomeController : Controller
{

    private readonly IPasswordResetService _passwordResetService;

    public HomeController(IPasswordResetService passwordResetService) 
    {
        _passwordResetService = passwordResetService;
    }

    public IActionResult Index()
    {
        return LocalRedirect("/Admin/TicketManagement/AssignedTickets");
    }

    public IActionResult Profile()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SendChangePasswordLink()
    {
        Guid currentUserID = (Guid) ViewBag.User.UserId;
        string linkSuffix = $"{HttpContext.Request.Protocol.Split('/')[0]}://{HttpContext.Request.Host}/Password/ResetPassword";
        
        PasswordResetSession passwordResetSession = new() {
            PasswordResetSessionID = Guid.NewGuid(),
            CreatedById = currentUserID,
            CreatedForUserId = currentUserID,
            CreatedAt = DateTime.Now,
            ForcedToResetPassword = false,
            LinkIsUsed = false,
        };
        await _passwordResetService.CreateResetSession(passwordResetSession, linkSuffix, false);

        return View();
    }

}
