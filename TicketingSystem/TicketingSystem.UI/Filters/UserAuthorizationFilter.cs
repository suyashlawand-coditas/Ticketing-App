using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.Core.Domain.Entities;
using System.Security.Claims;
using TicketingSystem.Core.Exceptions;
using Azure.Core;
using Azure;
using TicketingSystem.Core.Enums;

namespace TicketingSystem.UI.Filters;

public class UserAuthorizationFilter : IAsyncAuthorizationFilter
{

    private readonly IJwtService _jwtService;
    private readonly IUserServices _userServices;
    private readonly ILogger _logger;

    public UserAuthorizationFilter(IUserServices userServices, IJwtService jwtService, ILogger logger)
    {
        _userServices = userServices;
        _jwtService = jwtService;
        _logger = logger;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        string? jwtTokenFromHeader = context.HttpContext.Request.Cookies["Authorization"]!;
        string path = context.HttpContext.Request.Path;

        // Not Authenticated Private Route
        if (path.StartsWith("/Admin") || path.StartsWith("/Person") && String.IsNullOrEmpty(jwtTokenFromHeader))
        {
            context.Result = new RedirectToActionResult("Login", "Auth", new { });
        }
        else if (path.StartsWith("/Admin") || path.StartsWith("/Person") && !String.IsNullOrEmpty(jwtTokenFromHeader))
        {
            try
            {
                List<Claim> claims = _jwtService.VerifyJwtToken(jwtTokenFromHeader).ToList();
                string email = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)!.Value;
                string roleFromJwt = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)!.Value;

                Role role = (Role) Enum.Parse(typeof(Role), roleFromJwt, true);

                if (path.StartsWith("/Admin") && role == Core.Enums.Role.User)
                {
                    context.Result = new LocalRedirectResult("/User/Home");
                }
                else if (path.StartsWith("/User") && role == Core.Enums.Role.Admin)
                {
                    context.Result = new LocalRedirectResult("/Admin/Home");
                }

            }
            catch (BadJwtTokenException)
            {
                context.HttpContext.Response.Cookies.Delete("Authorization");
                throw;
            }
        }

    }
}
