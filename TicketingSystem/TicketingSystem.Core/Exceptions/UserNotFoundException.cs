using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Core.Exceptions
{
    public class UserNotFoundException : ArgumentException
    {
        public UserNotFoundException() : base("User not found.") { }
    }
}
