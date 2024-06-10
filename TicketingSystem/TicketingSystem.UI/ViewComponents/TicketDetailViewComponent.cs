using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.ServiceContracts;

namespace TicketingSystem.UI.ViewComponents;

[ViewComponent(Name = "TicketDetailViewComponent")]
public class TicketDetailViewComponent : ViewComponent
{

    private readonly ITicketService _ticketService;

    public TicketDetailViewComponent(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    public async Task<IViewComponentResult> InvokeAsync(Ticket ticket)
    {
        ViewBag.TicketStatusList = _ticketService.GetTicketStatuses();
        return await Task.FromResult((IViewComponentResult)View("TicketDetail", ticket));
    }
}
