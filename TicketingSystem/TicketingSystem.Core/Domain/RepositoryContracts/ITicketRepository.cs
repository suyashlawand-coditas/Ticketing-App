using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Enums;

namespace TicketingSystem.Core.Domain.RepositoryContracts
{
    public interface ITicketRepository
    {
        Task<Ticket> CreateTicket(Ticket Ticket);
        Task<Ticket> UpdateTicket(Ticket Ticket);
        Task<Ticket> UpdateTicketStatus(Guid TicketId, TicketStatus ticketStatus);
        Task<Ticket> GetTicketById(Guid TicketId);
        Task<List<Ticket>> GetAssignedAdminUnclosedTickets(Guid userId, int currentPage, int limit , string? search);
        Task<int> GetAssignedAdminUnclosedTicketCount(Guid userId, string? search);
        Task<List<Ticket>> GetUserRaisedUnclosedTicketList(Guid userId, int currentPage, int limit, string? search);
        Task<int> GetUserRaisedUnclosedTicketCount(Guid userId, string? search);
    }
}