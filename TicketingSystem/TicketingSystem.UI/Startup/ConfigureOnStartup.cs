using StackExchange.Redis;
using TicketingSystem.Infrastructure.DBContext;
using TicketingSystem.Infrastructure.Repository;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.Core.Services;
using TicketingSystem.UI.Middlewares;
using TicketingSystem.UI.Filters;
using Microsoft.EntityFrameworkCore;

namespace TicketingSystem.UI.Startup;

public static class ConfigureOnStartup
{
    public static void Configure(this WebApplicationBuilder builder)
    {
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
        ILogger logger = factory.CreateLogger("Program"); // TODO: Implement filebased logging.
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!);
        IDatabase db = redis.GetDatabase();

        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));
        builder.Services.AddControllersWithViews(
            opts =>
            {
                opts.Filters.Add<UserAuthenticationFilter>();
                opts.Filters.Add<AddUserModelToViewBagActionFilter>();
            }
            );

        builder.Services.AddSingleton<IDatabase>(db);
        builder.Services.AddSingleton<ILogger>(logger);
        builder.Services.AddSingleton<ExceptionHandlingMiddleware>();


        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<IDepartmentRepository, DepartmentRepository>();
        builder.Services.AddTransient<IUserRoleRepository, UserRoleRepository>();
        builder.Services.AddTransient<ITicketRepository, TicketRepository>();
        builder.Services.AddTransient<ITicketAssignmentRepository, TicketAssignmentRepository>();

        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<ITicketService, TicketService>();

        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
        builder.Services.AddTransient<IDepartmentService, DepartmentService>();
        builder.Services.AddSingleton<ICryptoService, CryptoService>();
        builder.Services.AddSingleton<IJwtService>(
            new JwtService(builder.Configuration["JwtConfigOptions:SecretKey"]!)
            );
    }
}
