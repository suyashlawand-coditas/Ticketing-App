using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.DTOs;

namespace TicketingSystem.Core.ServiceContracts
{
    public interface ITicketService
    {
        Task<TicketInfoDto> CreateAndAutoAssignTicket(CreateTicketDto createTicketDto, Guid raisedUserId);

        Task<List<Ticket>> GetAssignedAdminTickets(Guid userId, int currentPage, int limit, string? search);

        Task<int> GetAssignedAdminTicketCount(Guid userId, string? search);
    }
}
