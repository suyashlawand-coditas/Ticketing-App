using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.Domain.Entities;

namespace TicketingSystem.UI.ViewComponents;

[ViewComponent(Name = "TicketDetailViewComponent")]
public class TicketDetailViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(Ticket ticket)
    {
        return await Task.FromResult((IViewComponentResult) View("TicketDetail", ticket));
    }
}
