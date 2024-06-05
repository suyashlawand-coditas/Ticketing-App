using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.ServiceContracts;

namespace TicketingSystem.UI.ViewComponents
{
    [ViewComponent(Name = "ProfileViewComponent")]
    public class ProfileViewComponent: ViewComponent
    {

        private readonly IUserService _userService;
        public ProfileViewComponent(IUserService userService)
        {
            _userService = userService;             
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            Guid userId = (Guid)ViewBag.User.UserId;
            User? user = await _userService.FindUserByUserId(userId);

            return await Task.FromResult((IViewComponentResult)View("ProfileView", user));
        }
    }
}
