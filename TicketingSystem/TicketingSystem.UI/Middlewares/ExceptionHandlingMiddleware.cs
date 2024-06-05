using Microsoft.Data.SqlClient;
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
                _logger.LogWarning(ex.Message);
                throw;
            }
        }
    }
}
