namespace TicketingSystem.Core.Exceptions
{
    public class EntityNotFoundException: ArgumentException
    {
        public EntityNotFoundException(string entityName, string value = "") : base($"Entity {entityName} ({value}) Not Found") { }
    }
}
