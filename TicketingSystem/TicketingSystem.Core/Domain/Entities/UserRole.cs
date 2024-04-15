using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketingSystem.Core.Enums;

namespace TicketingSystem.Core.Domain.Entities;

public class UserRole
{
    [Key]
    public Guid RoleId { get; set; }

    [Required]
    public Role Role { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
