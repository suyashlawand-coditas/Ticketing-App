using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.UI.Models;
namespace TicketingSystem.UI.Areas.Admin.Controllers;

[Area("Admin")]
public class TicketManagementController : Controller
{
    private readonly ITicketService _ticketService;

    public TicketManagementController(ITicketService ticketService) { 
        _ticketService = ticketService;
    }


    public async Task<IActionResult> AssignedTickets([FromQuery] string? page, string? search, string? limit)
    {
        ViewUserDto viewUserDto = (ViewUserDto)ViewBag.User;
        Guid userId = viewUserDto.UserId;

        int currentPage = 1;
        int recordsLimit = 10;
        
        int.TryParse(page, out currentPage);
        if (currentPage < 1) {
            currentPage = 1;
        }
        int.TryParse(limit, out recordsLimit);
        if (recordsLimit < 1) {
            recordsLimit = 10;
        }


        int userTicketCount = await _ticketService.GetAssignedAdminTicketCount(userId, search);

        PagedViewModel<List<Ticket>> pagedViewModel = new PagedViewModel<List<Ticket>>() {
            CurrentPage = currentPage,
            PageSize = recordsLimit,
            TotalPages = (int)Math.Ceiling((decimal)userTicketCount / recordsLimit),
            ViewModel = await _ticketService.GetAssignedAdminTickets(userId, currentPage, recordsLimit, search),
        };

        return View(pagedViewModel);
    }

    public IActionResult YourTickets()
    {
        return View();
    }
}
