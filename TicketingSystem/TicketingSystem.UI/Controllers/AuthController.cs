﻿using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Enums;
namespace TicketingSystem.UI.Controllers;


[Route("[controller]/[action]")]
public class AuthController : Controller
{

    private readonly IJwtService _jwtService;
    private readonly ICryptoService _cryptoService;
    private readonly IUserService _userServices;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IJwtService jwtService, IUserService userServices, ICryptoService cryptoService, ILogger<AuthController> logger)
    {
        _jwtService = jwtService;
        _userServices = userServices;
        _cryptoService = cryptoService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromForm] LoginDto loginCredentials)
    {
        User? targetUser = await _userServices.FindUserByEmail(loginCredentials.Email);

        if (targetUser == null)
        {
            ViewBag.Error = $"User with email {loginCredentials.Email} doesnot exists.";
            return View();
        }
        else if (!targetUser.IsActive)
        {
            ViewBag.Error = $"User {loginCredentials.Email} is disabled by admin. Can't be accessed now.";
            return View();
        }
        else
        {
            bool isValidUser = _cryptoService.Verify(loginCredentials.Password, targetUser.PasswordHash, targetUser.PasswordSalt);
            if (!isValidUser)
            {
                ViewBag.Error = "Provided Password is incorrect";
                return View();
            }

            Response.Cookies.Append("Authorization", _jwtService.CreateJwtToken(targetUser));
            if (targetUser.Role.Role == Role.Admin)
            {
                return RedirectPermanent("/Admin/Home");
            }
            else if (targetUser.Role.Role == Role.User)
            {
                return RedirectPermanent("/Person/Home");
            }
            else
            {
                return Json(new { Error = "Role Not Assigned." });
            }
        }
    }

    [HttpGet]
    public IActionResult PerformLogout()
    {
        _logger.LogInformation("User Logged Out!");
        HttpContext.Response.Cookies.Delete("Authorization");
        return RedirectToAction("Login", "Auth");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
}
