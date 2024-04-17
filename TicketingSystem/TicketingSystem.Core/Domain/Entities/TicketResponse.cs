using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSystem.Core.Domain.Entities;

public class TicketResponse
{
    [Key]
    public Guid TicketResponseId { get; set; }

    public Guid TicketId { get; set; }

    [ForeignKey("TicketId")]
    public Ticket Ticket { get; set; } = null!;


    public Guid ResponseUserId { get; set; }

    [ForeignKey("ResponseUserId")]
    public User ResponseUser { get; set; } = null!;

    [DefaultValue(true)]
    public bool IsVisible { get; set; }

    public DateTime CreatedAt { get; set; }
}
