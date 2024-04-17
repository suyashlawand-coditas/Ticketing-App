using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Core.Exceptions;

public class CompletionStatusExistsException: ArgumentException
{
    public CompletionStatusExistsException() : base("There can be only one completion status") { }

}
