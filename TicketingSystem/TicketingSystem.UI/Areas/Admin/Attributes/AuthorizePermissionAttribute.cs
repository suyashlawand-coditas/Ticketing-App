using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.Enums;
using TicketingSystem.UI.Areas.Admin.Filters;

namespace TicketingSystem.UI.Areas.Admin.Attributes
{
    public class AuthorizePermissionAttribute : TypeFilterAttribute
    {
        public AuthorizePermissionAttribute(Permission permission) 
            : base(typeof(PermissionAuthorizationFilter))
        {
            Arguments = [permission];
        }
    }
}
