namespace TicketingSystem.Core.Exceptions
{
    public class UnauthorizedActionException: ArgumentException
    {
        public UnauthorizedActionException(): base("Sorry! You dont have access to perform this action!") { }
    }
}
