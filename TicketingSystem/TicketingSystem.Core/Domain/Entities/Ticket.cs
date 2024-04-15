using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketingSystem.Core.Enums;

namespace TicketingSystem.Core.Domain.Entities;

public class Ticket
{
    [Key]
    public Guid TicketId { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;
    
    [Required] 
    public Guid DepartmentId { get; set; }

    [ForeignKey("DepartmentId")]
    public Department Department { get; set; } = null!;

    [Required]
    public TPriority Priority { get; set; }

    public Guid TicketStatusId { get; set; }

    [ForeignKey("TicketStatusId")]
    public TicketStatus TicketStatus { get; set; } = null!;

    [DefaultValue(false)]
    public bool IsAutoAssigned { get; set; }

    public DateTime DueDate { get; set; }

    public Guid AssignedToUserId { get; set; }

    [ForeignKey("AssignedToUserId")]
    public User AssignedToUser { get; set; } = null!;

    public Guid RaisedById { get; set; }

    [ForeignKey("RaisedById")]
    public User RaisedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public ICollection<TicketResponse> TicketResponses { get; set; } = new List<TicketResponse>();

    public ICollection<TicketLog> Logs { get; set; } = new List<TicketLog>();
}