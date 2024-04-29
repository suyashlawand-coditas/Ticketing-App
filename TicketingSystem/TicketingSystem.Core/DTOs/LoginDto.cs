using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.Core.DTOs;

public class LoginDto
{
    [EmailAddress]
    [Required]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(maximumLength:15, MinimumLength = 6)]
    public string Password { get; set; } = null!;
}
