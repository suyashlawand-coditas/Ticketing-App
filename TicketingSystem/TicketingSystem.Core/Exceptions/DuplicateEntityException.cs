using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Core.Exceptions
{
    public class DuplicateEntityException: ArgumentException
    {
        public DuplicateEntityException(string entityName, string value = "") : base($"Duplicate {entityName} with value ({value}) already exists") { }
    }
}
