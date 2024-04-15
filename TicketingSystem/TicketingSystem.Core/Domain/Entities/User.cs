﻿using System.ComponentModel.DataAnnotations;
namespace TicketingSystem.Core.Domain.Entities;

public class User
{
    [Key]
    public Guid UserId { get; set; }

    [Required]
    public string FullName { get; set; } = null!;

    [Required]
    public string Phone { get; set; } = null!;

    [EmailAddress]
    [Required]
    public string Email { get; set; } = null!;

    public Guid DepartmentID { get; set; }
    public Department Department { get; set; } = null!;


    [Required]
    public string PasswordHash { get; set; } = null!;

    [Required]
    public string PasswordSalt { get; set; } = null!;

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    public UserSession? UserSession { get; set; }
    public UserRole? Role { get; set; }

    public ICollection<Ticket> AssignedTickets { get; set; } = [];

    public ICollection<Ticket> RaisedTickets { get; set; } = [];

    public ICollection<TicketResponse> TicketResponses { get; set; } = new List<TicketResponse>();

    public ICollection<AccessPermission> AccessPermissions { get; set; } = new List<AccessPermission>();

    public ICollection<TicketLog> TicketLogs { get; set; } = new List<TicketLog>();
}
