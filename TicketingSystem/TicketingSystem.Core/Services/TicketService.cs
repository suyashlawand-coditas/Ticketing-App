using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.Exceptions;
using TicketingSystem.Core.ServiceContracts;

namespace TicketingSystem.Core.Services
{
    public class TicketService : ITicketService
    {

        private readonly ITicketRepository _ticketRepository;
        private readonly ITicketAssignmentRepository _ticketAssignmentRepository;
        private readonly IUserRepository _userRepository;

        public TicketService(ITicketAssignmentRepository ticketAssignmentRepository, ITicketRepository ticketRepository, IUserRepository userRepository)
        {
            _ticketAssignmentRepository = ticketAssignmentRepository;
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
        }

        public async Task<TicketInfoDto> CreateAndAutoAssignTicket(CreateTicketDto createTicketDto, Guid raisedUserId)
        {
            User? assignmentAdmin = await _userRepository.GetAdminWithLeastTickets(createTicketDto.DepartmentId);

            if (assignmentAdmin != null)
            {
                Ticket ticket = new Ticket()
                {
                    TicketId = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Title = createTicketDto.Title,
                    Description = createTicketDto.Description,
                    DepartmentId = createTicketDto.DepartmentId,
                    DueDate = createTicketDto.DueDate,
                    IsActive = true,
                    RaisedById = raisedUserId,
                    Priority = createTicketDto.Priority,
                };

                TicketAssignment assignment = new TicketAssignment()
                {
                    TicketAssignmentId = Guid.NewGuid(),
                    AssignedUserId = assignmentAdmin.UserId,
                    CreatedAt = DateTime.Now,
                    TicketId = ticket.TicketId
                };

                return new TicketInfoDto()
                {
                    Ticket = await _ticketRepository.CreateTicket(ticket),
                    TicketAssignment = await _ticketAssignmentRepository.CreateTicketAssignment(assignment)
                };
            }
            else
            {
                throw new AssignmentAdminNotFound();
            }

        }

        public Task<int> GetAssignedAdminTicketCount(Guid userId, string? search)
        {
            return _ticketRepository.GetAssignedAdminTicketCount(userId, search);
        }

        public async Task<List<Ticket>> GetAssignedAdminTickets(Guid userId, int currentPage, int limit, string? searchIssue)
        {
            return await _ticketRepository.GetAssignedAdminTickets(userId, currentPage, limit, searchIssue);
        }
    }
}
