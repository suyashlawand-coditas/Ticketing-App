
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Core.Domain.Entities;

namespace TicketingSystem.Infrastructure.DBContext;

public class ApplicationDbContext: DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }

    private readonly bool _isMigration = true;

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

    public DbSet<TicketAssignment> TicketAssignments { get; set; }

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
            .WithOne(ticketLog => ticketLog.ActionUser)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<User>()
            .HasOne(user => user.Department)
            .WithMany(department => department.Users);

        modelBuilder.Entity<User>()
            .HasMany(user => user.TicketResponses)
            .WithOne(ticketResponse => ticketResponse.ResponseUser)
            .OnDelete(DeleteBehavior.NoAction);

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

        modelBuilder.Entity<Ticket>()
            .HasOne(ticket => ticket.RaisedBy)
            .WithMany(user => user.RaisedTickets);

        #endregion

        #region TicketAssignmentRelations

        modelBuilder.Entity<TicketAssignment>()
            .HasOne(ticketAssignment => ticketAssignment.AssignedUser)
            .WithMany(user => user.TicketAssignments)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<TicketAssignment>()
            .HasOne(ticketAssignment => ticketAssignment.Ticket)
            .WithOne(ticket => ticket.TicketAssignment)
            .OnDelete(DeleteBehavior.NoAction);

        #endregion

        #region UserCreation
        modelBuilder.Entity<UserCreation>()
            .HasOne(user => user.CreatedUser)
            .WithOne(user => user.UserCreation)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<UserCreation>()
            .HasOne(uc => uc.CreatorUser)
            .WithMany(user => user.CreatedUsers)
            .OnDelete(DeleteBehavior.NoAction);

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
