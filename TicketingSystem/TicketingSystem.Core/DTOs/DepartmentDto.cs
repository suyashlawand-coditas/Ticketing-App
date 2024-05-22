using System.ComponentModel.DataAnnotations;
using TicketingSystem.Core.Domain.Entities;

namespace TicketingSystem.Core.DTOs
{
    public class DepartmentDto
    {
        [Required]
        public Department Department { get; set; } = null!;

        public int PeopleCount { get; set; }
    }
}
