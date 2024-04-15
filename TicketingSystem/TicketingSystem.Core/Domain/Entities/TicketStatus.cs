using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TicketingSystem.Core.Enums;

namespace TicketingSystem.Core.Domain.Entities;

public class TicketStatus
{
    [Key]
    public Guid TicketStatusId { get; set; }

    [Required]
    public string Status { get; set; } = null!;

    [Required]
    public string StatusDescription { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    [DefaultValue(false)]
    public bool IsCompletionStatus { get; set; }

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
