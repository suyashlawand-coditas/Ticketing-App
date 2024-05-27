using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Exceptions;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.Core.Services;
using TicketingSystem.UI.Models;

namespace TicketingSystem.UI.Areas.Person.Controllers;


[Area("Person")]
public class TicketManagementController : Controller
{

    private readonly ITicketService _ticketService;
    private readonly ITicketResponseService _ticketResponseService;

    public TicketManagementController(ITicketService ticketService, ITicketResponseService ticketResponseService)
    {
        _ticketService = ticketService;
        _ticketResponseService = ticketResponseService;
    }

    public async Task<IActionResult> YourTickets([FromQuery] string? page, [FromQuery] string? search, [FromQuery] string? limit)
    {
        Guid userId = (Guid)ViewBag.User.UserId;

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

    [HttpGet("Person/TicketManagement/YourTickets/{id}")]
    public async Task<IActionResult> RaisedTicketDetail([FromRoute] Guid id)
    {
        Guid userId = (Guid)ViewBag.User.UserId;

        Ticket ticket = await _ticketService.GetTicketById(id);
        if (ticket.RaisedById != userId)
        {
            throw new UnauthorizedTicketAccessException();
        }
        else
        {
            ViewBag.TicketResponses = await _ticketResponseService.GetTicketResponseListByTicketId(id);
        }

        return View(ticket);
    }
}
