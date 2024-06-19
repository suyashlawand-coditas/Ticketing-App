using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Enums;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.Core.Services;

namespace TicketingSystem.UI.Areas.Admin.Filters
{
    public class PermissionAuthorizationFilter : IAsyncActionFilter
    {
        private readonly Permission _permission;
        private readonly ILogger<PermissionAuthorizationFilter> _logger;
        private readonly IAccessPermissionService _accessPermissionService;
        private readonly PermissionStoreService _permissionStoreService;
         
        public PermissionAuthorizationFilter(
            Permission permission,
            ILogger<PermissionAuthorizationFilter> logger,
            IAccessPermissionService accessPermissionService)
        {
            _accessPermissionService = accessPermissionService;
            _permission = permission;
            _logger = logger;
            _permissionStoreService = PermissionStoreService.Initialize();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            Controller controller = (Controller)context.Controller;
            ViewUserDto viewUser = (ViewUserDto)controller.ViewBag.User;
            Guid userId = viewUser.UserId;

            PermissionCacheDto? permissionCache = _permissionStoreService.GetPermissionFromStore(userId, _permission);

            if (permissionCache == null)
            {
                List<AccessPermission> permissions = await _accessPermissionService.GetAccessPermissionsOfUser(userId);
                List<AccessPermission> targetPermissions =
                    permissions
                        .Where(permission => permission.Permission == _permission || permission.Permission == Permission.MASTER_ACCESS)
                        .ToList();

                if (targetPermissions.Count() < 1)
                {
                    _logger.LogWarning($"User is not authorized to {_permission}");
                    context.Result = new LocalRedirectResult($"/Admin/AccessManagement/NoAccessPage?permission={_permission}");
                    return;
                }
                else
                {
                    _logger.LogWarning($"User is authorized to {_permission} (From Database)");
                    foreach (var targetPermission in targetPermissions)
                    {
                        _permissionStoreService.AddPermissionToStore(
                            new PermissionCacheDto()
                            {
                                UserId = targetPermission.UserId,
                                Permission = targetPermission.Permission,
                            }
                            );
                    }
                }
            }
            else
            {
                _logger.LogWarning($"User is authorized to {_permission} (From Singleton table)");
            }


            await next();
        }
    }
}
