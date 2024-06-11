using TicketingSystem.Core.Domain.Entities;

namespace TicketingSystem.Core.ServiceContracts;

public interface IEmailService
{
    public Task SendTicketResponseEmail(TicketResponse ticketResponse);

    public Task SendTicketStatusUpdateEmail(Ticket ticket);

    public Task SendTicketCreationEmail(Ticket ticket);

    public Task SendPasswordResetEmail(string email, string link);
}
