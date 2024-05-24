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

        public TicketRepository(ApplicationDbContext dbContext)
        {
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

            ticket.TicketStatus = Core.Enums.TicketStatus.Closed;
            await _dbContext.SaveChangesAsync();
            return ticket;
        }

        public async Task<int> GetAssignedAdminUnclosedTicketCount(Guid userId, string? search)
        {
            if (!String.IsNullOrEmpty(search))
            {
                return await _dbContext.TicketAssignments
                    .Include(tickt => tickt.Ticket)
                    .Where(ticketAssignment => ticketAssignment.AssignedUserId == userId || ticketAssignment.Ticket.RaisedBy!.FullName.Contains(search))
                    .Where(
                        ticketAssignment => ticketAssignment.Ticket.Title.Contains(search)
                            || ticketAssignment.Ticket.Description.Contains(search)
                            || ticketAssignment.Ticket.RaisedBy!.FullName.Contains(search)
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

        public async Task<List<Ticket>> GetAssignedAdminUnclosedTickets(Guid userId, int currentPage, int limit, string? search)
        {
            if (!String.IsNullOrEmpty(search))
            {
                return await _dbContext.TicketAssignments
                .Include(tickt => tickt.Ticket)
                .Include(tickt => tickt.Ticket.RaisedBy)
                .Where(ticketAssignment => ticketAssignment.AssignedUserId == userId)
                .Where(ticketAssignment => ticketAssignment.Ticket.TicketStatus != Core.Enums.TicketStatus.Closed)
                .Where(
                        ticketAssignment => ticketAssignment.Ticket.Title.Contains(search)
                            || ticketAssignment.Ticket.Description.Contains(search)
                            || ticketAssignment.Ticket.RaisedBy!.FullName.Contains(search)
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
                .Where(ticketAssignment => ticketAssignment.Ticket.TicketStatus != Core.Enums.TicketStatus.Closed)
                .Where(
                    ticketAssignment => ticketAssignment.AssignedUserId == userId
                )
                .Skip((currentPage - 1) * limit)
                .Take(limit)
                .Select(ticketAssignment => ticketAssignment.Ticket)
                .ToListAsync();
            }
        }

        public async Task<Ticket> GetTicketByTicketId(Guid ticketId)
        {
            Ticket? targetTicket = await _dbContext.Tickets
                .Include( ticket => ticket.Department )
                .Include( ticket => ticket.RaisedBy )
                .Include(ticket => ticket.TicketAssignment)
                .FirstOrDefaultAsync(ticket => ticket.TicketId == ticketId);
            if (targetTicket == null) throw new EntityNotFoundException<Ticket>();
            return targetTicket;
        }

        public async Task<int> GetUserRaisedUnclosedTicketCount(Guid userId, string? search)
        {
            if (!String.IsNullOrEmpty(search))
            {
                return await _dbContext.TicketAssignments
                .Where(ticketAssignment => ticketAssignment.Ticket.TicketStatus != Core.Enums.TicketStatus.Closed)
                .Where(ticketAssignment => ticketAssignment.Ticket.RaisedById == userId)
                .Where(
                        ticketAssignment => ticketAssignment.Ticket.Title.Contains(search)
                            || ticketAssignment.Ticket.Description.Contains(search)
                            || ticketAssignment.AssignedUser!.FullName.Contains(search)
                    )
                .CountAsync();
            }
            else
            {
                return await _dbContext.TicketAssignments
                .Include(tickt => tickt.Ticket)
                .Include(ticket => ticket.AssignedUser)
                .Where(ticketAssignment => ticketAssignment.Ticket.RaisedById == userId)
                .CountAsync();
            }
        }

        public async Task<List<Ticket>> GetUserRaisedUnclosedTicketList(Guid userId, int currentPage, int limit, string? search)
        {
            if (!String.IsNullOrEmpty(search))
            {
                return await _dbContext.TicketAssignments
                .Include(ticket => ticket.AssignedUser)
                .Include(tickt => tickt.Ticket)
                .Include(ticket => ticket.Ticket.Department)
                .Include(ticket => ticket.Ticket.TicketAssignment)
                .Include(ticket => ticket.Ticket.TicketAssignment!.AssignedUser)
                .Where(ticketAssignment => ticketAssignment.Ticket.TicketStatus != Core.Enums.TicketStatus.Closed)
                .Where(ticketAssignment => ticketAssignment.Ticket.RaisedById == userId)
                .Where(
                        ticketAssignment => ticketAssignment.Ticket.Title.Contains(search)
                            || ticketAssignment.Ticket.Description.Contains(search)
                            || ticketAssignment.AssignedUser!.FullName.Contains(search)
                    )
                .Skip((currentPage - 1) * limit)
                .Take(limit)
                .Select(ticketAssignment => ticketAssignment.Ticket)
                .ToListAsync();
            }
            else
            {
                return await _dbContext.TicketAssignments
                .Include(ticket => ticket.AssignedUser)
                .Include(tickt => tickt.Ticket)
                .Include(ticket => ticket.Ticket.Department)
                .Include(ticket => ticket.Ticket.TicketAssignment)
                .Include(ticket => ticket.Ticket.TicketAssignment!.AssignedUser)
                .Where(ticketAssignment => ticketAssignment.Ticket.RaisedById == userId)
                .Skip((currentPage - 1) * limit)
                .Take(limit)
                .Select(ticketAssignment => ticketAssignment.Ticket)
                .ToListAsync();
            }
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