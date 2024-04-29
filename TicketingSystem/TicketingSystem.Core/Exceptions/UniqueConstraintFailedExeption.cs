namespace TicketingSystem.Core.Exceptions;

public class UniqueConstraintFailedExeption: ArgumentException
{
    public UniqueConstraintFailedExeption(string message): base(message) { }
}
