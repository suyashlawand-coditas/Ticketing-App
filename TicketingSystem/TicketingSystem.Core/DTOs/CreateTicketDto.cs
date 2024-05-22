using System.ComponentModel.DataAnnotations;
using TicketingSystem.Core.Enums;

namespace TicketingSystem.Core.DTOs
{
    public class CreateTicketDto
    {
        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        public Guid DepartmentId { get; set; }

        public TPriority Priority { get; set; }

        public DateTime DueDate { get; set; }
    }
}