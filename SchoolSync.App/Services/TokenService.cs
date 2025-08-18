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

        // Ensure the user's Role navigation property is loaded
        if (user.Role == null)
        {
            // If not loaded, throw an exception to prevent issuing a token without a role
            throw new InvalidOperationException($"User with ID {user.Id} does not have a Role loaded. RoleId: {user.RoleId}");
        }

        var roleName = user.Role.Name;
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim("role", roleName)
        };

        // Create the JWT token with issuer, audience, claims, expiration, and signing credentials
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiresInMinutes"]!)),
            signingCredentials: creds
        );

        // Serialize the token to a string and return it
        string ret = new JwtSecurityTokenHandler().WriteToken(token);
        return ret;
    }
}
