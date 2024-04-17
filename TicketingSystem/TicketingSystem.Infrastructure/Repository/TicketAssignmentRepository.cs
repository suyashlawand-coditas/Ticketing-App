using Microsoft.EntityFrameworkCore;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.Exceptions;
using TicketingSystem.Infrastructure.DBContext;

namespace TicketingSystem.Infrastructure.Repository;

public class TicketAssignmentRepository : ITicketAssignmentRepository
{

    private readonly ApplicationDbContext _dbContext;

    public TicketAssignmentRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TicketAssignment> CreateTicketAssignment(TicketAssignment ticketAssignment)
    {
        await _dbContext.TicketAssignments.AddAsync(ticketAssignment);
        await _dbContext.SaveChangesAsync();
        return ticketAssignment;
    }

    public async Task<bool> DeleteTicketAssignment(TicketAssignment ticketAssignment)
    {
        TicketAssignment? targetTicketAssignment = await _dbContext.TicketAssignments.FirstOrDefaultAsync(tktAssignment => tktAssignment.TicketAssignmentId == ticketAssignment.TicketAssignmentId);

        if (targetTicketAssignment == null) throw new EntityNotFoundException<TicketAssignment>();
        _dbContext.Remove(targetTicketAssignment);

        int count = await _dbContext.SaveChangesAsync();
        return count == 1;
    }

    public Task<TicketAssignment> UpdateTicketAssignment(TicketAssignment ticketAssignment)
    {
        throw new NotImplementedException();
    }
}
