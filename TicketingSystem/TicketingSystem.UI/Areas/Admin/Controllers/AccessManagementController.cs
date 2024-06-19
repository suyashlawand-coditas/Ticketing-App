using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Enums;
using TicketingSystem.Core.Exceptions;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.Core.Services;
using TicketingSystem.UI.Areas.Admin.Attributes;
using TicketingSystem.Core.DTOs;

namespace TicketingSystem.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccessManagementController : Controller
    {
        private readonly IAccessPermissionService _accessPermissionService;
        private readonly IPermissionStoreService _permissionStoreService;

        public AccessManagementController(IAccessPermissionService accessPermissionService)
        {
            _accessPermissionService = accessPermissionService;
            _permissionStoreService = PermissionStoreService.Initialize();
        }

        [HttpPost("GrantAccess/{userId}")]
        [AuthorizePermission(Permission.MASTER_ACCESS)]
        public async Task<IActionResult> GrantAccess([FromRoute] Guid userId, [FromForm] Permission accessPermission)
        {
            Guid currentUserId = (Guid)ViewBag.User.UserId;
            AccessPermission access = await _accessPermissionService.CreateAccessPermissionForUser(userId, currentUserId, accessPermission);
            _permissionStoreService.AddPermissionToStore(new PermissionCacheDto() { 
                UserId = access.UserId,
                Permission = accessPermission
            }); 
            return LocalRedirect($"/Admin/UserManagement/EditByUserId/{userId}");
        }


        [HttpPost("RevokeAccess/{userId}/{accessId}")]
        [AuthorizePermission(Permission.MASTER_ACCESS)]
        public async Task<IActionResult> RevokeAccess([FromRoute] Guid accessId, [FromRoute] Guid userId)
        {
            AccessPermission? accessPermission = await _accessPermissionService.GetAccessPermissionById(accessId);
            if (accessPermission != null) 
            {
                _permissionStoreService.RevokePermissionFromStore(accessPermission.UserId, accessPermission.Permission);
                await _accessPermissionService.DeleteAccessPermissionById(accessId);
                return LocalRedirect($"/Admin/UserManagement/EditByUserId/{userId}");
            }
            else
            {
                throw new EntityNotFoundException(nameof(AccessPermission));
            }
        }

        
        public IActionResult NoAccessPage([FromQuery] Permission permission)
        {
            ViewBag.AccessInfo = permission;
            return View();
        }

    }
}
