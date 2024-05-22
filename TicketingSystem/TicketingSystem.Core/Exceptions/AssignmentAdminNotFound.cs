using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Core.Exceptions
{
    public class AssignmentAdminNotFound: ArgumentException
    {
        public AssignmentAdminNotFound() : base("Admin not found to assign ticket.") { }
    }
}
