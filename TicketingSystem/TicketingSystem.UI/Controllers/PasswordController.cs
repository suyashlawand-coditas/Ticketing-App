using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.ServiceContracts;

namespace TicketingSystem.UI.Controllers
{
    [Route("[controller]/[action]")]
    public class PasswordController : Controller
    {
        private readonly IPasswordResetService _passwordResetService;

        public PasswordController(IPasswordResetService passwordResetService)
        {
            _passwordResetService = passwordResetService;
        }

        [HttpGet("{passwordHash}")]
        public async Task<IActionResult> ResetPassword([FromRoute] string passwordHash)
        {
            PasswordResetSession? passwordResetSession = await _passwordResetService.FindPasswordResetSessionByIdHash(passwordHash);
            if (passwordResetSession == null)
            {
                ViewBag.Error = "Invalid Link";
            }
            else
            {
                ViewBag.Email = passwordResetSession.CreatedForUser.Email;
            }

            return View();
        }

        [HttpPost("{passwordHash}")]
        public async Task<IActionResult> ResetPassword([FromRoute] string passwordHash, [FromForm] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View(changePasswordDto);
            }

            PasswordResetSession? passwordResetSession = await _passwordResetService.FindPasswordResetSessionByIdHash(passwordHash);
            if (passwordResetSession == null)
            {
                return Json("Error");
            }

            await _passwordResetService.ChangePassword(changePasswordDto.Password, passwordResetSession);
            return Json("Successfully changed password!");
        }
    }
}
