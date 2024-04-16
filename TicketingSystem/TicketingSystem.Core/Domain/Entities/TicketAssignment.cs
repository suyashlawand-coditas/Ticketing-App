using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSystem.Core.Domain.Entities;

public class TicketAssignment
{
    [Key]
    public Guid TicketAssignmentId { get; set; }

    [Required]
    public Guid AssignedUserId { get; set; }

    [ForeignKey("AssignedUserId")]
    public User AssignedUser { get; set; } = null!;

    [Required]
    public Guid TicketId { get; set; }

    [ForeignKey("TicketId")]
    public Ticket Ticket { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

}
