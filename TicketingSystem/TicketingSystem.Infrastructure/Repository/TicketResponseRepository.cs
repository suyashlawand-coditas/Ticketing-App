using Microsoft.EntityFrameworkCore;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.Exceptions;
using TicketingSystem.Infrastructure.DBContext;

namespace TicketingSystem.Infrastructure.Repository;

public class TicketResponseRepository : ITicketResponseRepository
{

    private readonly ApplicationDbContext _dbContext;

    public TicketResponseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TicketResponse> ChangeTicketResponseVisiblity(Guid ticketResponseId, bool isVisible)
    {
        TicketResponse? ticketResponse = await _dbContext.TicketResponses.FirstOrDefaultAsync(
                (tktResponse) => tktResponse.TicketResponseId == ticketResponseId
            ) ?? throw new EntityNotFoundException<TicketResponse>();

        ticketResponse.IsVisible = isVisible;
        await _dbContext.SaveChangesAsync();

        return ticketResponse;
    }

    public async Task<TicketResponse> CreateTicketResponse(TicketResponse ticketResponse)
    {
        _dbContext.TicketResponses.Add(ticketResponse);
        await _dbContext.SaveChangesAsync();

        return ticketResponse;
    }

    public async Task<bool> DeleteTicketResponse(Guid ticketResponseId)
    {
        TicketResponse ticketResponse = await _dbContext.TicketResponses.FirstOrDefaultAsync(
               (tktResponse) => tktResponse.TicketResponseId == ticketResponseId) ?? throw new EntityNotFoundException<TicketResponse>();

        _dbContext.TicketResponses.Remove(ticketResponse);
        int affectedCount = await _dbContext.SaveChangesAsync();

        return affectedCount > 0;
    }

    public async Task<TicketResponse> GetTicketResponseById(Guid ticketResponseId)
    {
        return await _dbContext.TicketResponses.FirstOrDefaultAsync(
                (ticketResponse) => ticketResponse.TicketResponseId == ticketResponseId
            ) ?? throw new EntityNotFoundException<TicketResponse>();
    }

    public async Task<List<TicketResponse>> GetTicketResponseListByTicketId(Guid ticketId)
    {
        return await _dbContext.TicketResponses
            .Include(ticketResponse => ticketResponse.ResponseUser)
            .Where(ticketResponse => ticketResponse.TicketId == ticketId)
            .OrderByDescending( ticketResponse => ticketResponse.CreatedAt )
            .ToListAsync();
    }
}
