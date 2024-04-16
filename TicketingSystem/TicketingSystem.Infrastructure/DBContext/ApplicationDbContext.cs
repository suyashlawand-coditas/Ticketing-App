
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using System.Reflection.PortableExecutable;
using TicketingSystem.Core.Domain.Entities;

namespace TicketingSystem.Infrastructure.DBContext;

public class ApplicationDbContext: DbContext
{

    private readonly bool _isMigration = false;

    public DbSet<User> Users { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketResponse> TicketResponses { get; set; }
    public DbSet<TicketLog> TicketLogs { get; set; }
    public DbSet<TicketStatus> TicketStatuses { get; set; }
    public DbSet<AccessPermission> AccessPermissions { get; set; }
    public DbSet<UserCreation> UserCreations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region UserRelations

        modelBuilder.Entity<User>()
            .HasOne(user => user.UserSession)
            .WithOne(userSession => userSession.User);

        modelBuilder.Entity<User>()
            .HasOne(user => user.Role)
            .WithOne(role => role.User);

        modelBuilder.Entity<User>()
            .HasMany(user => user.AccessPermissions)
            .WithOne(accessPermission => accessPermission.User);

        modelBuilder.Entity<User>()
            .HasMany(user => user.TicketLogs)
            .WithOne(ticketLog => ticketLog.ActionUser);

        modelBuilder.Entity<User>()
            .HasOne(user => user.Department)
            .WithMany(department => department.Users);

        modelBuilder.Entity<User>()
            .HasMany(user => user.TicketResponses)
            .WithOne(ticketResponse => ticketResponse.ResponseUser);

        modelBuilder.Entity<User>()
            .HasMany(user => user.AssignedTickets)
            .WithOne(assignedTickets => assignedTickets.AssignedToUser);

        modelBuilder.Entity<User>()
            .HasMany(user => user.RaisedTickets)
            .WithOne(ticket => ticket.RaisedBy);

        modelBuilder.Entity<User>()
            .HasMany(user => user.CreatedUsers)
            .WithOne(userCreation => userCreation.CreatorUser);

        modelBuilder.Entity<User>()
            .HasOne(user => user.UserCreation)
            .WithOne(userCreation => userCreation.CreatedUser);

        #endregion

        #region DepartmentRelations

        modelBuilder.Entity<Department>()
            .HasMany(deptartment => deptartment.Tickets)
            .WithOne( ticktet => ticktet.Department);

        #endregion

        #region TicketRelations

        modelBuilder.Entity<Ticket>()
            .HasMany(ticket => ticket.TicketResponses)
            .WithOne(ticketResponse => ticketResponse.Ticket);

        modelBuilder.Entity<Ticket>()
            .HasMany(ticket => ticket.Logs)
            .WithOne(log => log.Ticket);

        #endregion

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (_isMigration)
        {
            optionsBuilder.UseSqlServer(
                "Data Source=5602-LAP-0347\\SQLEXPRESS01;Initial Catalog=ticketing_app_db;Integrated Security=True;Trust Server Certificate=True"
            );
        }
    }

}
