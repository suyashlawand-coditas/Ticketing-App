using TicketingSystem.Core.Domain.Entities;

namespace TicketingSystem.Core.Domain.RepositoryContracts
{
    public interface ITicketAssignmentRepository
    {
        Task<TicketAssignment> CreateTicketAssignment(TicketAssignment ticketAssignment);
        Task<TicketAssignment> UpdateTicketAssignment(TicketAssignment ticketAssignment);
        Task<bool> DeleteTicketAssignment(TicketAssignment ticketAssignment);
    }
}
