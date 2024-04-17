using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Infrastructure.DBContext;

namespace TicketingSystem.Infrastructure.Repository;

public class TicketLogRepository : ITicketLogRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TicketLogRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TicketLog> CreateTicket(TicketLog ticketLog)
    {
        _dbContext.TicketLogs.Add(ticketLog);   
        await _dbContext.SaveChangesAsync();
        return ticketLog;
    }

}
