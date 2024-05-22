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
    private readonly IDatabase _redisDB;
    private readonly IJwtService _jwtService;

    public AddUserModelToViewBagActionFilter(IUserService userService, IDatabase redisDB, IJwtService jwtService)
    {
        _redisDB = redisDB;
        _userService = userService;
        _jwtService = jwtService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        string? authToken = context.HttpContext.Request.Cookies["Authorization"];

        if (!String.IsNullOrEmpty(authToken))
        {
            // skip redndant verification
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

                if (await _redisDB.KeyExistsAsync($"authToken-{tokenId}"))
                {
                    string? requestUserDtoInJson = await _redisDB.StringGetAsync($"authToken-{tokenId}");
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
                        await _redisDB.StringSetAsync($"authToken-{tokenId}", requestUserDtoInJson);
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
