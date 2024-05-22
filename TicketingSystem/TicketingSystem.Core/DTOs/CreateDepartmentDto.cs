using System.ComponentModel.DataAnnotations;
using TicketingSystem.Core.Domain.Entities;

namespace TicketingSystem.Core.DTOs
{
    public class CreateDepartmentDto
    {
        [Required]
        public string Name { get; set; } = null!;

        public Department ToDepartment()
        {
            return new Department()
            {
                DepartmentId = Guid.NewGuid(),
                Name = this.Name,
                CreatedAt = DateTime.Now
            };
        }
    }
}
