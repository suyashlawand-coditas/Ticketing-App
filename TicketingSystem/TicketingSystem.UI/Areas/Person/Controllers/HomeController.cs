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

        public async Task<IActionResult> Index([FromQuery] string? page, [FromQuery] string? search, [FromQuery] string? limit)
        {
            Guid userId = (Guid) ViewBag.User.UserId;

            int currentPage = 1;
            int recordsLimit = 10;

            int.TryParse(page, out currentPage);
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            int.TryParse(limit, out recordsLimit);
            if (recordsLimit < 1)
            {
                recordsLimit = 10;
            }


            int userTicketCount = await _ticketService.GetUserRaisedUnclosedTicketCount(userId, search);

            PagedViewModel<List<Ticket>> pagedViewModel = new PagedViewModel<List<Ticket>>()
            {
                CurrentPage = currentPage,
                PageSize = recordsLimit,
                TotalPages = (int)Math.Ceiling((decimal)userTicketCount / recordsLimit),
                ViewModel = await _ticketService.GetUserRaisedUnclosedTicketList(userId, currentPage, recordsLimit, search),
            };
            return View(pagedViewModel);
        }

        public IActionResult ProfileSettings() { 
            return View();
        }

        public IActionResult Profile() {
            return View();
        }
    }
}
