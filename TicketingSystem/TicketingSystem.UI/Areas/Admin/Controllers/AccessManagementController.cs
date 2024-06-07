using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.Enums;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.UI.Areas.Admin.Attributes;

namespace TicketingSystem.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccessManagementController : Controller
    {
        private readonly IAccessPermissionService _accessPermissionService;
        public AccessManagementController(IAccessPermissionService accessPermissionService)
        {
            _accessPermissionService = accessPermissionService;
        }

        [HttpPost("GrantAccess/{userId}")]
        [AuthorizePermission(Permission.MASTER_ACCESS)]
        public async Task<IActionResult> GrantAccess([FromRoute] Guid userId, [FromForm] Permission accessPermission)
        {
            Guid currentUserId = (Guid)ViewBag.User.UserId;
            await _accessPermissionService.CreateAccessPermissionForUser(userId, currentUserId, accessPermission);
            return LocalRedirect($"/Admin/UserManagement/EditByUserId/{userId}");
        }


        [HttpPost("RevokeAccess/{userId}/{accessId}")]
        [AuthorizePermission(Permission.MASTER_ACCESS)]
        public async Task<IActionResult> RevokeAccess([FromRoute] Guid accessId, [FromRoute] Guid userId)
        {
            Guid currentUserId = (Guid)ViewBag.User.UserId;
            await _accessPermissionService.DeleteAccessPermissionById(accessId);
            return LocalRedirect($"/Admin/UserManagement/EditByUserId/{userId}");
        }

        
        public IActionResult NoAccessPage([FromQuery] Permission permission)
        {
            ViewBag.AccessInfo = permission;
            return View();
        }

    }
}
