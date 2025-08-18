using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SchoolSync.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace SchoolSync.App.Services;

// Implementation of the token service
public class TokenService(IConfiguration config) : ITokenService
{
    // Store configuration for accessing JWT settings
    private readonly IConfiguration _config = config;

    // Generates a JWT token for the specified user, including their role as a claim
    public string GenerateToken(User user)
    {
        // Retrieve JWT settings from configuration (appsettings.json)
        var jwtSettings = _config.GetSection("Jwt");

        // Create the security key and signing credentials for the token
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var roleIdint = user.RoleId;
        var roleId = roleIdint.ToString();
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(ClaimTypes.Role, roleId) // Use ClaimTypes.Role for standard role claim
            // new Claim("Role", Role.Name) this is not the standard
        };
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiresInMinutes"]!)),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
