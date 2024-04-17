using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
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
            Ticket? ticket =  await _dbContext.Tickets.FindAsync(TicketId);
            if (ticket == null) throw new EntityNotFoundException<Ticket>();

            ticket.IsActive = false;
            await _dbContext.SaveChangesAsync();
            return ticket;
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
