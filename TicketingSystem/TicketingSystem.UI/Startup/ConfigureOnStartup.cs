using TicketingSystem.Infrastructure.DBContext;
using TicketingSystem.Infrastructure.Repository;
using TicketingSystem.Core.Domain.RepositoryContracts;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.Core.Services;
using TicketingSystem.UI.Middlewares;
using TicketingSystem.UI.Filters;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace TicketingSystem.UI.Startup;

public static class ConfigureOnStartup
{
    public static void Configure(this WebApplicationBuilder builder)
    {
        //Serilog
        builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) => {
            loggerConfiguration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services);
        });
        builder.Services.AddHttpLogging(options =>
        {
            options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
        });

        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));
        builder.Services.AddControllersWithViews((opts) => {
                // Global Filters
                opts.Filters.Add<UserAuthenticationFilter>();
                opts.Filters.Add<AddUserModelToViewBagActionFilter>();
                opts.Filters.Add<NoCacheFilter>();
            }
        );

        // Repositories
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<IDepartmentRepository, DepartmentRepository>();
        builder.Services.AddTransient<IUserRoleRepository, UserRoleRepository>();
        builder.Services.AddTransient<ITicketRepository, TicketRepository>();
        builder.Services.AddTransient<ITicketAssignmentRepository, TicketAssignmentRepository>();
        builder.Services.AddTransient<ITicketResponseRepository, TicketResponseRepository>();
        builder.Services.AddTransient<IAccessPermissionRepository, AccessPermissionRepository>();

        // Services
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<ITicketService, TicketService>();
        builder.Services.AddTransient<IDepartmentService, DepartmentService>();
        builder.Services.AddTransient<ITicketResponseService, TicketResponseService>();
        builder.Services.AddTransient<IAccessPermissionService, AccessPermissionService>();

        // Other Services
        builder.Services.AddSingleton<ICryptoService, CryptoService>();
        builder.Services.AddSingleton<ICacheService>(new CacheService(builder.Configuration.GetConnectionString("RedisConnection")!));
        builder.Services.AddSingleton<ExceptionHandlingMiddleware>();
        builder.Services.AddSingleton<IEmailService>(new EmailService(
            builder.Configuration["TICKETING_APP_EMAIL"]!,
            builder.Configuration["TICKETING_APP_EMAIL_PASSWORD"]!,
            builder.Environment.IsProduction()
            ));
        builder.Services.AddSingleton<IJwtService>(
            new JwtService(builder.Configuration["JwtConfigOptions:SecretKey"]!)
            );

        builder.Services.AddSignalR();
        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
    }
}
