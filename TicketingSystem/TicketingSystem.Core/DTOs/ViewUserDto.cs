using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Enums;

namespace TicketingSystem.Core.DTOs
{
    public class ViewUserDto
    {
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(12)]
        [MinLength(10)]
        public string Phone { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public bool IsActive { get; set; }

        public bool IsNewUser { get; set; }

        public Guid DepartmentId { get; set; }

        public string DepartmentName { get; set; } = null!;

        public Role Role { get; set; }

        public DateTime CreatedAt { get; set; }

        public static ViewUserDto FromUser(User user) {
            return new ViewUserDto() {
                UserId = user.UserId,
                FullName = user.FullName,
                Phone = user.Phone,
                Email = user.Email,
                IsActive = user.IsActive,
                IsNewUser = user.ForceToResetPassword,
                DepartmentId = user.DepartmentId,
                Role = user.Role.Role,
                DepartmentName = user.Department.Name,
                CreatedAt = user.CreatedAt,
            };
        }
    }
}