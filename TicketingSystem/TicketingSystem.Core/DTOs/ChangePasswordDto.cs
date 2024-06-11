using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.Core.DTOs
{
    public class ChangePasswordDto
    {
        [Required]
        [MaxLength(15)]
        [MinLength(6)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        [Compare(nameof(Password), ErrorMessage = "Passwords don't match.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
