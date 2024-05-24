using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.Core.Exceptions;

namespace TicketingSystem.UI.Filters;

public class AdminAssignedTicketFilter : IAsyncActionFilter
{

    private readonly ITicketService _ticketService;

    public AdminAssignedTicketFilter(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        Controller controller = (Controller)context.Controller;
        Guid userId = (Guid) controller.ViewBag.User.UserId;
        Guid ticketId = Guid.Parse(Convert.ToString(context.HttpContext.Request.RouteValues["id"]!)!);
        
        Ticket ticket = await _ticketService.GetTicketById(ticketId);
        if (ticket.TicketAssignment!.AssignedUserId != userId)
        {
            throw new UnauthorizedTicketAccessException();
        }

        controller.ViewBag.Ticket = ticket;

        await next();
    }
}
