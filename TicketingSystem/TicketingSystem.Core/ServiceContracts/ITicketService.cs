using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.DTOs;

namespace TicketingSystem.Core.ServiceContracts
{
    public interface ITicketService
    {
        Task<TicketInfoDto> CreateAndAutoAssignTicket(CreateTicketDto createTicketDto, Guid raisedUserId);
    }
}
