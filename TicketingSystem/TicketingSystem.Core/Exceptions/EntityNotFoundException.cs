namespace TicketingSystem.Core.Exceptions
{
    public class EntityNotFoundException<T>: ArgumentException
    {
        public EntityNotFoundException() : base($"{nameof(T)} Not Found") { }
    }
}
