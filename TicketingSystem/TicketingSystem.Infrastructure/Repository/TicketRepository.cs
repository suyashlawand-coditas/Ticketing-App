using Microsoft.EntityFrameworkCore;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.Exceptions;
using TicketingSystem.Infrastructure.DBContext;

namespace TicketingSystem.Infrastructure.Repository
{
    public class TicketRepository : ITicketRepository
    {

        private readonly ApplicationDbContext _dbContext;

        public TicketRepository(ApplicationDbContext dbContext) { 
            _dbContext = dbContext; 
        }

        public async Task<Ticket> CreateTicket(Ticket ticket)
        {
            await _dbContext.Tickets.AddAsync(ticket);
            return ticket;
        }

        public async Task<Ticket> DeactivateTicket(Guid TicketId)
        {
            Ticket? ticket = await _dbContext.Tickets.FindAsync(TicketId);
            if (ticket == null) throw new EntityNotFoundException<Ticket>();

            ticket.IsActive = false;
            await _dbContext.SaveChangesAsync();
            return ticket;
        }

        public async Task<int> GetAssignedAdminUnClosedTicketCount(Guid userId, string? search)
        {
            if (!String.IsNullOrEmpty(search))
            {
                return await _dbContext.TicketAssignments
                    .Include(tickt => tickt.Ticket)
                    .Where(ticketAssignment => ticketAssignment.AssignedUserId == userId)
                    .Where(
                        ticketAssignemnt => ticketAssignemnt.Ticket.Title.Contains(search) ||
                            ticketAssignemnt.Ticket.Description.Contains(search)
                    )
                    .CountAsync();
            }
            else
            {
                return await _dbContext.TicketAssignments.Where(
                    ticketAssignment => ticketAssignment.AssignedUserId == userId
                ).CountAsync();
            }
        }

        public async Task<List<Ticket>> GetAssignedAdminUnClosedTickets(Guid userId, int currentPage, int limit, string? search)
        {
            if (!String.IsNullOrEmpty(search))
            {
                return await _dbContext.TicketAssignments
                .Include(tickt => tickt.Ticket)
                .Include(tickt => tickt.Ticket.RaisedBy)
                .Where(ticketAssignment => ticketAssignment.AssignedUserId == userId)
                .Where(
                        ticketAssignemnt => ticketAssignemnt.Ticket.Title.Contains(search) ||
                            ticketAssignemnt.Ticket.Description.Contains(search)
                    )
                .Skip((currentPage - 1) * limit)
                .Take(limit)
                .Select(ticketAssignment => ticketAssignment.Ticket)
                .ToListAsync();
            }
            else
            {
                return await _dbContext.TicketAssignments
                .Include(tickt => tickt.Ticket)
                .Include(tickt => tickt.Ticket.RaisedBy)
                .Where(
                    ticketAssignment => ticketAssignment.AssignedUserId == userId
                )
                .Skip((currentPage - 1) * limit)
                .Take(limit)
                .Select(ticketAssignment => ticketAssignment.Ticket)
                .ToListAsync();
            }
        }

        public async Task<Ticket> GetTicketByTicketId(Guid TicketId)
        {
            Ticket? targetTicket = await _dbContext.Tickets.FirstOrDefaultAsync(ticket => ticket.TicketId == TicketId);
            if (targetTicket == null) throw new EntityNotFoundException<Ticket>();
            return targetTicket;
        }

        public async Task<Ticket> UpdateTicket(Ticket ticket)
        {
            Ticket? ticketToUpdate = await _dbContext.Tickets.FirstOrDefaultAsync((tkt) => tkt.TicketId == ticket.TicketId);

            if (ticketToUpdate == null) throw new EntityNotFoundException<Ticket>();

            ticketToUpdate = ticket;
            await _dbContext.SaveChangesAsync();

            return ticketToUpdate;
        }
    }
}