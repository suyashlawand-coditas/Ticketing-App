using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.DTOs;

namespace TicketingSystem.Core.ServiceContracts
{
    public interface ITicketService
    {
        Task<TicketInfoDto> CreateAndAutoAssignTicket(CreateTicketDto createTicketDto, Guid raisedUserId, string? updatedFilePath);

        Task<List<Ticket>> GetAssignedAdminUnclosedTickets(Guid userId, int currentPage, int limit, string? search);

        Task<int> GetAssignedAdminUnclosedTicketCount(Guid userId, string? search);

        Task<List<Ticket>> GetUserRaisedUnclosedTicketList(Guid userId, int currentPage, int limit, string? search);

        Task<int> GetUserRaisedUnclosedTicketCount(Guid userId, string? search);
    }
}
