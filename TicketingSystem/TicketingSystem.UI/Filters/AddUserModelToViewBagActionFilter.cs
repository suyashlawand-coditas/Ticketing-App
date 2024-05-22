using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StackExchange.Redis;
using System.Security.Claims;
using System.Text.Json;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.ServiceContracts;

namespace TicketingSystem.UI.Filters;

public class AddUserModelToViewBagActionFilter : IAsyncActionFilter
{
    private readonly IUserService _userService;
    private readonly ICacheService _cacheService;
    private readonly IJwtService _jwtService;

    public AddUserModelToViewBagActionFilter(ICacheService cacheService, IUserService userService, IJwtService jwtService)
    {
        _cacheService = cacheService;
        _userService = userService;
        _jwtService = jwtService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        string? authToken = context.HttpContext.Request.Cookies["Authorization"];

        if (!String.IsNullOrEmpty(authToken))
        {
            IEnumerable<Claim> jwtClaims = _jwtService.VerifyJwtToken(authToken);
            string tokenId = jwtClaims.First(claim => claim.Type == ClaimTypes.Sid).Value;
            string userId = jwtClaims.First(claim => claim.Type == ClaimTypes.UserData).Value;

            if (authToken == null)
            {
                context.Result = new LocalRedirectResult("/Auth/PerformLogout");
            }
            else
            {
                Controller controller = (Controller)context.Controller;

                if (await _cacheService.DoesExist($"authToken-{tokenId}"))
                {
                    string? requestUserDtoInJson = await _cacheService.Get($"authToken-{tokenId}") as string; 
                    ViewUserDto? requestUserDto = JsonSerializer.Deserialize<ViewUserDto>(requestUserDtoInJson!);
                    if (requestUserDto == null)
                    {
                        User? requestUser = await _userService.FindUserByUserId(Guid.Parse(userId));
                        if (requestUser == null)
                        {
                            context.Result = new LocalRedirectResult("/Auth/PerformLogout");
                        }
                    }
                    else
                    {
                        controller.ViewBag.User = requestUserDto;
                    }
                    await next();
                }
                else
                {
                    User? requestUser = await _userService.FindUserByUserId(Guid.Parse(userId));
                    if (requestUser == null)
                    {
                        context.Result = new LocalRedirectResult("/Auth/PerformLogout");
                    }
                    else
                    {
                        ViewUserDto requestUserDto = ViewUserDto.FromUser(requestUser);
                        controller.ViewBag.User = requestUser;
                        string requestUserDtoInJson = JsonSerializer.Serialize<ViewUserDto>(requestUserDto);
                        await _cacheService.Set($"authToken-{tokenId}", requestUserDtoInJson);
                    }
                    await next();
                }
            }
        }
        else
        {
            await next();
        }
    }
}
