using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TicketingSystem.Core.Domain.Entities;

public class UserCreation
{
    [Key]
    public Guid UserCreationId { get; set; }

    public Guid CreatedUserId { get; set; }

    [ForeignKey("CreatedUserId")]
    public User CreatedUser { get; set; } = null!;

    public Guid CreatorUserId { get; set; }

    [ForeignKey("CreatorUserId")]
    public User CreatorUser { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
