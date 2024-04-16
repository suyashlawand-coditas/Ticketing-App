using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketingSystem.Core.Enums;

namespace TicketingSystem.Core.Domain.Entities;

public class AccessPermission
{
    [Key]
    public Guid PermissionId { get; set; }

    [Required]
    public Permission Permission { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
