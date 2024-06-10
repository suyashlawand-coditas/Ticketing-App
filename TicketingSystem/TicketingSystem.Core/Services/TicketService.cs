using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.Enums;
using TicketingSystem.Core.Exceptions;
using TicketingSystem.Core.ServiceContracts;

namespace TicketingSystem.Core.Services
{
    public class TicketService : ITicketService
    {

        private readonly ITicketRepository _ticketRepository;
        private readonly ITicketAssignmentRepository _ticketAssignmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;   

        public TicketService(ITicketAssignmentRepository ticketAssignmentRepository, ITicketRepository ticketRepository, IUserRepository userRepository, IEmailService emailService)
        {
            _ticketAssignmentRepository = ticketAssignmentRepository;
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task<TicketInfoDto> CreateAndAutoAssignTicket(CreateTicketDto createTicketDto, Guid raisedUserId, string? updatedFilePath)
        {
            User? assignmentAdmin = await _userRepository.GetAdminWithLeastTickets(createTicketDto.DepartmentId);
            User? raisedByUser = await _userRepository.FindUserByUserId(raisedUserId);
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
                    RaisedBy = raisedByUser,
                    RaisedById = raisedUserId,
                    Priority = createTicketDto.Priority,
                    FilePath = updatedFilePath,
                };

                TicketAssignment assignment = new TicketAssignment()
                {
                    TicketAssignmentId = Guid.NewGuid(),
                    AssignedUserId = assignmentAdmin.UserId,
                    AssignedUser = assignmentAdmin,
                    CreatedAt = DateTime.Now,
                    TicketId = ticket.TicketId
                };

                ticket.TicketAssignment = assignment;
                _ = _emailService.SendTicketCreationEmail(ticket);


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

        public Task<int> GetAssignedAdminUnclosedTicketCount(Guid userId, string? search)
        {
            return _ticketRepository.GetAssignedAdminUnclosedTicketCount(userId, search);
        }

        public async Task<List<Ticket>> GetAssignedAdminUnclosedTickets(Guid userId, int currentPage, int limit, string? searchIssue)
        {
            return await _ticketRepository.GetAssignedAdminUnclosedTickets(userId, currentPage, limit, searchIssue);
        }

        public async Task<Ticket> GetTicketById(Guid ticketId)
        {
            return await _ticketRepository.GetTicketById(ticketId);
        }

        public List<TicketStatus> GetTicketStatuses()
        {
            List<TicketStatus> ticketStatuses = Enum.GetValues(typeof(TicketStatus)).Cast<TicketStatus>().ToList();
            ticketStatuses.Remove(TicketStatus.Assigned);
            return ticketStatuses;
        }

        public async Task<int> GetUserRaisedUnclosedTicketCount(Guid userId, string? search)
        {
            return await _ticketRepository.GetUserRaisedUnclosedTicketCount(userId, search);
        }

        public async Task<List<Ticket>> GetUserRaisedUnclosedTicketList(Guid userId, int currentPage, int limit, string? search)
        {
            return await _ticketRepository.GetUserRaisedUnclosedTicketList(userId, currentPage, limit, search);
        }

        public async Task<Ticket> UpdateTicket(Ticket ticket)
        {
            _ = _emailService.SendTicketStatusUpdateEmail(ticket);
            return await _ticketRepository.UpdateTicket(ticket);
        }

        public async Task<Ticket> UpdateTicketStatus(Guid ticketId, TicketStatus ticketStatus)
        {
            return await _ticketRepository.UpdateTicketStatus(ticketId, ticketStatus);
        }
    }
}
