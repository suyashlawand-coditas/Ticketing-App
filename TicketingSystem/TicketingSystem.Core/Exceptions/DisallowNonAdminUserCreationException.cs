using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Core.Exceptions
{
    public class DisallowNonAdminUserCreationException : ArgumentException
    {
        public DisallowNonAdminUserCreationException(): base ("Non admin user cannot create new user.") {}
    }
}
