using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.Exceptions;
using TicketingSystem.Core.ServiceContracts;

namespace TicketingSystem.Core.Services
{
    public class TicketResponseService : ITicketResponseService
    {
        private readonly ITicketResponseRepository _ticketResponseRepository;
        private readonly ITicketService _ticketService;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public TicketResponseService(ITicketResponseRepository ticketResponseRepository, ITicketService ticketService, IEmailService emailService) 
        { 
            _ticketResponseRepository = ticketResponseRepository;
            _ticketService = ticketService;
            _emailService = emailService;
        }

        public async Task<TicketResponse> CreateTicketResponse(Guid creatorUserId, Guid ticketId, string responseMessage)
        {
            Ticket ticket = await _ticketService.GetTicketById(ticketId);

            if (ticket.RaisedById == creatorUserId || ticket.TicketAssignment?.AssignedUserId == creatorUserId)
            {
                TicketResponse ticketResponse = new TicketResponse() { 
                    TicketResponseId = Guid.NewGuid(),
                    TicketId = ticketId,
                    ResponseUserId = creatorUserId,
                    Ticket = ticket,
                    IsVisible = true,
                    CreatedAt = DateTime.Now,
                    ResponseMessage = responseMessage
                };

                _ = _emailService.SendTicketResponseEmail(ticketResponse);
                return await _ticketResponseRepository.CreateTicketResponse(ticketResponse);
            } else
            {
                throw new UnauthorizedTicketAccessException();
            }
        }

        public async Task<bool> DeleteTicketResponse(Guid ticketResponseId)
        {
            return await _ticketResponseRepository.DeleteTicketResponse(ticketResponseId);
        }

        public async Task<TicketResponse> GetTicketResponseById(Guid ticketResponseId)
        {
            return await _ticketResponseRepository.GetTicketResponseById(ticketResponseId);
        }

        public async Task<List<TicketResponse>> GetTicketResponseListByTicketId(Guid ticketId)
        {
            return await _ticketResponseRepository.GetTicketResponseListByTicketId(ticketId);
        }
    }
}
