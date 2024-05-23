using Microsoft.Data.SqlClient;
using TicketingSystem.Core.Exceptions;

namespace TicketingSystem.UI.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (SqlException sqlException)
            {
                if (sqlException.InnerException!.Message.Contains("IX_Users_Email"))
                {
                    throw new UniqueConstraintFailedExeption("User with this email already exists");
                }
                else if (sqlException.InnerException.Message.Contains("IX_Users_Phone"))
                {
                    throw new UniqueConstraintFailedExeption("User with this phone already exists");
                }
                else
                {
                    throw new UniqueConstraintFailedExeption("Entity with this phone already exists");
                }

            }
            catch
            {
                throw;
            }
        }
    }
}
