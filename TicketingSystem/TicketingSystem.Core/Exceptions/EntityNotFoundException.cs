namespace TicketingSystem.Core.Exceptions
{
    public class EntityNotFoundException<T>: ArgumentException
    {
        public EntityNotFoundException() : base($"Entity Not Found") { }
    }
}
