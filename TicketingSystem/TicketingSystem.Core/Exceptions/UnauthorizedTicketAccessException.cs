namespace TicketingSystem.Core.Exceptions
{
    public class UnauthorizedTicketAccessException: ArgumentException
    {
        public UnauthorizedTicketAccessException() : base("Trying to access unauthorized ticket.") { }
    }
}
