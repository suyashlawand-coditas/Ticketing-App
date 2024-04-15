using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.Core.Domain.Entities;

public class Department
{
    [Key]
    public Guid DepartmentId { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public ICollection<User> Users { get; set; } = new List<User>();

    [Required]
    public DateTime CreatedAt { get; set; }

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
