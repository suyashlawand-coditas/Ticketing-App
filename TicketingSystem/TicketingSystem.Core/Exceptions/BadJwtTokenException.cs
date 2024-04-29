namespace TicketingSystem.Core.Exceptions;

public class BadJwtTokenException : ArgumentException
{
    public BadJwtTokenException() : base("Unable to verify JWT Token") { }
}
