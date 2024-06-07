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
    public DbSet<AccessPermission> AccessPermissions { get; set; }
    public DbSet<UserCreation> UserCreations { get; set; }

    public DbSet<TicketAssignment> TicketAssignments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(user => user.Phone)
            .IsUnique();

        modelBuilder.Entity<Department>()
            .HasIndex(dept => dept.Name)
            .IsUnique();

        #region UserRelations

        modelBuilder.Entity<UserSession>()
            .HasOne(userSession => userSession.User)
            .WithOne(user => user.UserSession)
            .HasForeignKey<UserSession>(userSession => userSession.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(role => role.User)
            .WithOne(user => user.Role)
            .HasForeignKey<UserRole>(usrRole => usrRole.UserId);
            

        modelBuilder.Entity<AccessPermission>()
            .HasOne(accessPermission => accessPermission.User)
            .WithMany( user => user.AccessPermissions)
            .HasForeignKey(accessPermission => accessPermission.UserId);

        modelBuilder.Entity<AccessPermission>()
            .HasOne(accessPermission => accessPermission.GrantedBy)
            .WithMany(user => user.GrantedPermissions)
            .HasForeignKey(accessPermission => accessPermission.GrantedById)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<AccessPermission>()
            .HasIndex(accessPermission => new { accessPermission.Permission, accessPermission.UserId })
            .IsUnique();

        modelBuilder.Entity<TicketLog>()
            .HasOne(ticketLog => ticketLog.ActionUser)
            .WithMany(user => user.TicketLogs)
            .HasForeignKey(ticketLog => ticketLog.ActionUserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Department>()
            .HasMany(dept => dept.Users)
            .WithOne(user => user.Department)
            .HasForeignKey(d => d.DepartmentId)
            .IsRequired(false);


        modelBuilder.Entity<TicketResponse>()
            .HasOne(ticketResponse => ticketResponse.ResponseUser)
            .WithMany(user => user.TicketResponses)
            .HasForeignKey(ticketRes => ticketRes.ResponseUserId)
            .OnDelete(DeleteBehavior.NoAction);

        #endregion

        #region DepartmentRelations

        modelBuilder.Entity<Ticket>()
            .HasOne(ticktet => ticktet.Department)
            .WithMany(deptartment => deptartment.Tickets)
            .HasForeignKey( dept => dept.DepartmentId);

        #endregion

        #region TicketRelations

        modelBuilder.Entity<TicketResponse>()
            .HasOne(ticketResponse => ticketResponse.Ticket)
            .WithMany(ticket => ticket.TicketResponses)
            .HasForeignKey(ticketRes => ticketRes.TicketId);


        modelBuilder.Entity<TicketLog>()
            .HasOne(log => log.Ticket)
            .WithMany(ticket => ticket.Logs)
            .HasForeignKey(tkt => tkt.TicketId);

        modelBuilder.Entity<Ticket>()
            .HasOne(ticket => ticket.RaisedBy)
            .WithMany(user => user.RaisedTickets)
            .HasForeignKey(tkt => tkt.RaisedById);

        #endregion

        #region TicketAssignmentRelations

        modelBuilder.Entity<TicketAssignment>()
            .HasOne(ticketAssignment => ticketAssignment.AssignedUser)
            .WithMany(user => user.TicketAssignments)
            .HasForeignKey(tktAssignment => tktAssignment.AssignedUserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<TicketAssignment>()
            .HasOne(ticketAssignment => ticketAssignment.Ticket)
            .WithOne(ticket => ticket.TicketAssignment)
            .HasForeignKey<TicketAssignment>(ta => ta.TicketId)
            .OnDelete(DeleteBehavior.NoAction);

        #endregion

        #region UserCreation
        modelBuilder.Entity<UserCreation>()
            .HasOne(userCreation => userCreation.CreatedUser)
            .WithOne(user => user.UserCreation)
            .HasForeignKey<UserCreation>(user => user.UserCreationId) // Assuming User has a property UserCreationId
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<UserCreation>()
            .HasOne(userCreation => userCreation.CreatorUser)
            .WithMany(user => user.CreatedUsers)
            .HasForeignKey(userCreation => userCreation.CreatorUserId) // Assuming UserCreation has a property CreatorUserId
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
