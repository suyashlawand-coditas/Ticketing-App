﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TicketingSystem.Core.ServiceContracts;
using System.Security.Claims;
using TicketingSystem.Core.Exceptions;
using TicketingSystem.Core.Enums;

namespace TicketingSystem.UI.Filters;

public class UserAuthenticationFilter : IAuthorizationFilter
{
    private readonly IJwtService _jwtService;

    public UserAuthenticationFilter(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        string? jwtTokenFromHeader = context.HttpContext.Request.Cookies["Authorization"]!;
        string path = context.HttpContext.Request.Path;

        if ((path.StartsWith("/Admin") || path.StartsWith("/Person") || path.StartsWith("/Ticket")) && String.IsNullOrEmpty(jwtTokenFromHeader))
        {
            context.Result = new RedirectToActionResult("Login", "Auth", new { });
        }
        else if ((path.StartsWith("/Admin") || path.StartsWith("/Person") || path.StartsWith("/Ticket") || path.Equals("/Auth/Login")) && !String.IsNullOrEmpty(jwtTokenFromHeader))
        {
            try
            {
                List<Claim> claims = _jwtService.VerifyJwtToken(jwtTokenFromHeader).ToList();
                string email = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)!.Value;
                string roleFromJwt = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)!.Value;

                Role role = (Role)Enum.Parse(typeof(Role), roleFromJwt, true);

                if (!path.StartsWith("/Ticket"))
                {
                    if (path.StartsWith("/Admin") && role == Role.User)
                    {
                        context.Result = new LocalRedirectResult("/Person/Home");
                    }
                    else if (path.StartsWith("/Person") && role == Role.Admin)
                    {
                        context.Result = new LocalRedirectResult("/Admin/Home");
                    }
                }

                if (path.Equals("/Auth/Login"))
                {
                    context.Result =
                        role == Role.User ?
                            new LocalRedirectResult("/Person/Home")
                        : new LocalRedirectResult("/Admin/Home");
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
