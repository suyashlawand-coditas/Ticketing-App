using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
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

    [AllowNull]
    [DefaultValue(null)]
    public Guid? GrantedById { get; set; }

    [ForeignKey("GrantedById")]
    public User? GrantedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
