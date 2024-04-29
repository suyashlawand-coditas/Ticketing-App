using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Core.Exceptions
{
    public class IncorrectPasswordException: ArgumentException
    {
        public IncorrectPasswordException() : base("Provided password is incorrect") { }
    }
}
