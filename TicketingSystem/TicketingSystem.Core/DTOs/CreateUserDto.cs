using System.ComponentModel.DataAnnotations;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Enums;

namespace TicketingSystem.Core.DTOs;
public class CreateUserDto
{
    [StringLength(40)]
    public string FullName { get; set; } = null!;

    [Phone]
    [Required]
    [MaxLength(12)]
    public string Phone { get; set; } = null!;

    [EmailAddress]
    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public Guid DepartmentID { get; set; }

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public Role Role { get; set; }

    public User ToUser()
    {
        return new User() { 
            UserId = Guid.NewGuid(),
            FullName = FullName,
            Phone = Phone,
            Email = Email,
            DepartmentID = DepartmentID,
            CreatedAt = DateTime.Now,
            IsActive = true,
        };
    }

}
