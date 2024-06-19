using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Core.Exceptions;

namespace TicketingSystem.UI.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {

        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}", ex.Message);

                if (ex.InnerException != null)
                {
                    _logger.LogError("Inner exception: {InnerExceptionMessage}", ex.InnerException.Message);
                }

                // Throw a user-friendly exception with a description of what went wrong
                throw;
            }

        }
    }
}
