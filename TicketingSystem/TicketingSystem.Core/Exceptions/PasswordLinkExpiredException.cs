namespace TicketingSystem.Core.Exceptions
{
    public class PasswordLinkExpiredException: ArgumentException
    {
        public PasswordLinkExpiredException() : base("Link to reset the password is expired.") { }
    }
}
