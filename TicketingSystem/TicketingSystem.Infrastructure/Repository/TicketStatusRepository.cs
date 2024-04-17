using Microsoft.EntityFrameworkCore;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.Exceptions;
using TicketingSystem.Infrastructure.DBContext;

namespace TicketingSystem.Infrastructure.Repository;

public class TicketStatusRepository : ITicketStatusRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TicketStatusRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TicketStatus> AddTicketStatus(TicketStatus ticketStatus)
    {
        if (ticketStatus.IsCompletionStatus)
        {
            bool isNotAllowed = _dbContext.TicketStatuses.Where(tktStatus => tktStatus.IsCompletionStatus == true).Count() > 0;
            if (isNotAllowed) throw new CompletionStatusExistsException();
        }

        _dbContext.TicketStatuses.Add(ticketStatus);
        await _dbContext.SaveChangesAsync();
        return ticketStatus;
    }

    public async Task<TicketStatus> UpdateTicketStatus(TicketStatus ticketStatus)
    {
        TicketStatus status = await _dbContext.TicketStatuses.FirstOrDefaultAsync(
            searchTicketStatus => searchTicketStatus.TicketStatusId == ticketStatus.TicketStatusId
         ) ?? throw new EntityNotFoundException<TicketStatus>();

        if (ticketStatus.IsCompletionStatus)
        {
            bool isNotAllowed = _dbContext.TicketStatuses.Where(tktStatus => tktStatus.IsCompletionStatus == true).Count() > 0;
            if (isNotAllowed) throw new CompletionStatusExistsException();
        }

        status = ticketStatus;
        await _dbContext.SaveChangesAsync();

        return status;
    }
}
