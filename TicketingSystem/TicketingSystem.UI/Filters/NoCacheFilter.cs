using Microsoft.AspNetCore.Mvc.Filters;

namespace TicketingSystem.UI.Filters
{
    public class NoCacheFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await next();
            var response = context.HttpContext.Response;

            response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            response.Headers["Expires"] = "-1";
            response.Headers["Pragma"] = "no-cache";

        }
    }
}
