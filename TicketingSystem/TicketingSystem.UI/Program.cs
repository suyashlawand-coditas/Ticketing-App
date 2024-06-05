using TicketingSystem.UI.Middlewares;
using TicketingSystem.UI.Startup;
using TicketingSystem.UI.Hubs;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Configure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Error");
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.MapHub<TicketResponseHub>("/ticket-response");

app.UseEndpoints((endpoints) => {
    endpoints.MapControllerRoute(
        name: "AdminAreaRoute",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}",
        defaults: new { area = "Admin" }
    );
});


app.Run();