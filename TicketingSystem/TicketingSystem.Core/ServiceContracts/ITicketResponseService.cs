using TicketingSystem.Core.Domain.Entities;

namespace TicketingSystem.Core.ServiceContracts
{
    public interface ITicketResponseService
    {
        Task<TicketResponse> CreateTicketResponse(Guid creatorUserId, Guid ticketId, string responseMessage);
        Task<List<TicketResponse>> GetTicketResponseListByTicketId(Guid ticketId);
        Task<bool> DeleteTicketResponse(Guid ticketResponseId);
        Task<TicketResponse> GetTicketResponseById(Guid ticketResponseId);
    }
}
