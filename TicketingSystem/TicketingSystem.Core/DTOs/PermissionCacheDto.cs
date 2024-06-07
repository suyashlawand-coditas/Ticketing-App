using TicketingSystem.Core.Enums;

namespace TicketingSystem.Core.DTOs
{
    public class PermissionCacheDto : IEquatable<PermissionCacheDto>
    {
        public Guid UserId { get; set; }
        public Permission Permission { get; set; }

        public bool Equals(PermissionCacheDto? other)
        {
            return other!.UserId == UserId && other.Permission == Permission;
        }
    }
}
