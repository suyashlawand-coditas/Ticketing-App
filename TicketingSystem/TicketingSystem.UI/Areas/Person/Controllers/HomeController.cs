using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.UI.Models;

namespace TicketingSystem.UI.Areas.Person.Controllers
{
    [Area("Person")]
    public class HomeController : Controller
    {

        private readonly ITicketService _ticketService;

        public HomeController(ITicketService ticketService)
        {
            _ticketService = ticketService;
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
    }
}
