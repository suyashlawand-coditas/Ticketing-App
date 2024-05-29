using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.Enums;
using TicketingSystem.Core.Exceptions;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.UI.Models;
namespace TicketingSystem.UI.Areas.Admin.Controllers;

[Area("Admin")]
public class TicketManagementController : Controller
{
    private readonly ITicketService _ticketService;
    private readonly ITicketResponseService _ticketResponseService;

    public TicketManagementController(ITicketService ticketService, ITicketResponseService ticketResponseService)
    {
        _ticketService = ticketService;
        _ticketResponseService = ticketResponseService;
    }


    [HttpPost("Admin/TicketManagement/AssignedTickets/UpdateTicketStatus/{id}")]
    public async Task<IActionResult> UpdateTicketStatus([FromRoute] Guid id, [FromForm] TicketStatus ticketStatus)
    {
        ViewUserDto userDto = (ViewUserDto)ViewBag.User;
        Ticket ticket = await _ticketService.GetTicketById(id);

        if (userDto.Role == Role.Admin && ticket.TicketAssignment!.AssignedUserId == userDto.UserId)
        {
            ticket.TicketStatus = ticketStatus;
            await _ticketService.UpdateTicket(ticket);
        }

        return LocalRedirect($"/Admin/TicketManagement/AssignedTickets/{id}");
    }

    [HttpGet]
    public async Task<IActionResult> AssignedTickets([FromQuery] string? page, [FromQuery] string? search, [FromQuery] string? limit)
    {
        ViewUserDto viewUserDto = (ViewUserDto)ViewBag.User;


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


        int userTicketCount = await _ticketService.GetAssignedAdminUnclosedTicketCount(viewUserDto.UserId, search);

        PagedViewModel<List<Ticket>> pagedViewModel = new PagedViewModel<List<Ticket>>()
        {
            CurrentPage = currentPage,
            PageSize = recordsLimit,
            TotalPages = (int)Math.Ceiling((decimal)userTicketCount / recordsLimit),
            ViewModel = await _ticketService.GetAssignedAdminUnclosedTickets(viewUserDto.UserId, currentPage, recordsLimit, search),
        };

        return View(pagedViewModel);
    }

    [HttpGet]
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

    [HttpGet("Admin/TicketManagement/AssignedTickets/{id}")]
    public async Task<IActionResult> AssignedTicketDetail([FromRoute] Guid id)
    {
        Guid userId = (Guid)ViewBag.User.UserId;

        Ticket ticket = await _ticketService.GetTicketById(id);
        if (ticket.TicketAssignment!.AssignedUserId != userId)
        {
            throw new UnauthorizedTicketAccessException();
        }
        else
        {
            ViewBag.TicketResponses = await _ticketResponseService.GetTicketResponseListByTicketId(id);
        }

        return View(ticket);
    }

    [HttpGet("Admin/TicketManagement/YourTickets/{id}")]
    public async Task<IActionResult> YourTicketDetail([FromRoute] Guid id)
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
