using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Core.Domain.Entities;

namespace TicketingSystem.Core.Domain.RepositoryContracts;

public interface ITicketResponseRepository
{
    Task<TicketResponse> CreateTicketResponse(TicketResponse ticketResponse);
    Task<TicketResponse> ChangeTicketResponseVisiblity(Guid ticketResponseId, bool isVisible);
    Task<TicketResponse> GetTicketResponseById(Guid ticketResponseId);
    Task<bool> DeleteTicketResponse(Guid ticketResponseId);
}
