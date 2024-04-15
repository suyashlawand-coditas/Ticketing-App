
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketingSystem.Core.Enums;

namespace TicketingSystem.Core.Domain.Entities;

public class TicketLog
{
    public Guid TicketLogId { get; set; }

    public TicketLogAction TicketLogAction { get; set; }

    [Required]
    public Guid TicketId { get; set; }

    [ForeignKey("TicketId")]
    public Ticket Ticket { get; set; } = null!;

    [Required]
    public Guid ActionUserId { get; set; }
    
    public User ActionUser { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
