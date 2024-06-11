using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSystem.Core.Domain.Entities;

public class PasswordResetSession
{
    public Guid PasswordResetSessionID { get; set; }

    public Guid CreatedById { get; set; }

    [ForeignKey(nameof(CreatedById))]
    public User CreatedBy { get; set; } = null!;

    public Guid CreatedForUserId { get; set; }

    [ForeignKey(nameof(CreatedForUserId))]
    public User CreatedForUser { get; set; } = null!;

    [DefaultValue(false)]
    public bool ForcedToResetPassword { get; set; }

    [DefaultValue(false)]
    public bool LinkIsUsed { get; set; }

    public DateTime CreatedAt { get; set; }
}