using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Core.Domain.Entities;

namespace TicketingSystem.Core.Domain.RepositoryContracts
{
    public interface ITicketLogRepository
    {
        Task<TicketLog> CreateTicket(TicketLog ticketLog);
    }
}
