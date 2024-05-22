using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Core.Domain.Entities;

namespace TicketingSystem.Core.DTOs
{
    public class TicketInfoDto
    {
        public Ticket Ticket { get; set; }
        public TicketAssignment TicketAssignment { get; set; }
    }
}
