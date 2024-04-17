﻿using TicketingSystem.Core.Domain.Entities;

namespace TicketingSystem.Core.Domain.RepositoryContracts
{
    public interface ITicketRepository
    {
        Task<Ticket> CreateTicket(Ticket Ticket);
        Task<Ticket> UpdateTicket(Ticket Ticket);
        Task<Ticket> DeactivateTicket(Guid TicketId);
        Task<Ticket> GetTicketByTicketId(Guid TicketId);
    }
}