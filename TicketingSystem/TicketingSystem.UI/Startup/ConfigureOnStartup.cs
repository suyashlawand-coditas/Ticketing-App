using TicketingSystem.Infrastructure.DBContext;
using TicketingSystem.Infrastructure.Repository;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.Core.Services;

namespace TicketingSystem.UI.Startup;

public static class ConfigureOnStartup
{
    public static void Configure(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>();
        builder.Services.AddControllersWithViews();
        
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<IDepartmentRepository, DepartmentRepository>();
        builder.Services.AddTransient<IUserRoleRepository, UserRoleRepository>();
        
        builder.Services.AddTransient<IUserServices, UserService>();
        builder.Services.AddTransient<IDepartmentService, DepartmentService>();
    } 
}
