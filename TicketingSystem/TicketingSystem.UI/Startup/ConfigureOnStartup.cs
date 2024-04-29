using TicketingSystem.Infrastructure.DBContext;
using TicketingSystem.Infrastructure.Repository;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.Core.Services;
using TicketingSystem.UI.Middlewares;
using TicketingSystem.UI.Filters;

namespace TicketingSystem.UI.Startup;

public static class ConfigureOnStartup
{
    public static void Configure(this WebApplicationBuilder builder)
    {
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
        ILogger logger = factory.CreateLogger("Program");

        builder.Services.AddDbContext<ApplicationDbContext>();
        builder.Services.AddControllersWithViews(
                opts => opts.Filters.Add<UserAuthorizationFilter>()
            );

        builder.Services.AddSingleton<ILogger>(logger);
        builder.Services.AddSingleton<ExceptionHandlingMiddleware>();
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<IDepartmentRepository, DepartmentRepository>();
        builder.Services.AddTransient<IUserRoleRepository, UserRoleRepository>();


        builder.Services.AddTransient<IUserServices, UserService>();
        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
        builder.Services.AddTransient<IDepartmentService, DepartmentService>();
        builder.Services.AddSingleton<ICryptoService, CryptoService>();
        builder.Services.AddSingleton<IJwtService>(
            new JwtService(builder.Configuration["JwtConfigOptions:SecretKey"]!)
            );
    }
}
