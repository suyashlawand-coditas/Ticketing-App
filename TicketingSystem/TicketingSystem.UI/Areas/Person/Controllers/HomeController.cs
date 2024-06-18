using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.Core.Services;
using TicketingSystem.UI.Models;

namespace TicketingSystem.UI.Areas.Person.Controllers
{
    [Area("Person")]
    public class HomeController : Controller
    {

        private readonly ITicketService _ticketService;
        private readonly IPasswordResetService _passwordResetService;

        public HomeController(ITicketService ticketService, IPasswordResetService passwordResetService)
        {
            _ticketService = ticketService;
            _passwordResetService = passwordResetService;
        }

        public IActionResult Index()
        {
            return LocalRedirect("/Person/TicketManagement/YourTickets");
        }

        public IActionResult ProfileSettings() { 
            return View();
        }

        public IActionResult Profile() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendChangePasswordLink()
        {
            Guid currentUserID = (Guid)ViewBag.User.UserId;
            string linkSuffix = $"{HttpContext.Request.Protocol.Split('/')[0]}://{HttpContext.Request.Host}/Password/ResetPassword";

            PasswordResetSession passwordResetSession = new()
            {
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
}
