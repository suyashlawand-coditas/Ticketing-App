using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Exceptions;
using TicketingSystem.Core.ServiceContracts;

namespace TicketingSystem.Core.Services;

public class JwtService : IJwtService
{

    private readonly string _secretKey;
    public JwtService(string secretKey)
    {
        _secretKey = "This is my custom Secret key for authentication in Debug mode";
    }

    public string CreateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Create a JWT token
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public List<Claim> VerifyJwtToken(string token)
    {
        try
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            // Validate the token
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var validationParameters = new TokenValidationParameters
            {
                // Set the parameters to validate the token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = false, // You can set it to true if you want to validate the issuer
                ValidateAudience = false, // You can set it to true if you want to validate the audience
                ClockSkew = TimeSpan.Zero // You can adjust the acceptable clock skew
            };

            // Try to validate the token and extract the claims
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            // You can now access the claims
            return principal.Claims.ToList();
        }
        catch (SecurityTokenException ex)
        {
            // Token validation failed
            Console.WriteLine($"Token validation failed: {ex.Message}");
            throw new BadJwtTokenException();
        }
    }
}
