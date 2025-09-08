using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SchoolSync.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SchoolSync.Domain.IServices;
namespace SchoolSync.App.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    // Store configuration for accessing JWT settings
    private readonly IConfiguration _config = config;

    // Simple in-memory token blacklist - in production, use Redis or database
    private static readonly HashSet<string> _revokedTokens = new();
    private static readonly Dictionary<int, DateTime> _userTokenRevocationTimes = new();

    // Generates a JWT token for the specified user, including their role as a claim
    public string GenerateToken(User user)
    {
        // Retrieve JWT settings from configuration (appsettings.json)
        var jwtSettings = _config.GetSection("Jwt");

        // Create the security key and signing credentials for the token
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var roleName = user.Role?.Name ?? "Unknown";
        var issuedAt = DateTime.UtcNow;
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(ClaimTypes.Role, roleName),
            new Claim(JwtRegisteredClaimNames.Iat, ((DateTimeOffset)issuedAt).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
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

    public Task RevokeTokensForUserAsync(int userId)
    {
        _userTokenRevocationTimes[userId] = DateTime.UtcNow;
        return Task.CompletedTask;
    }

    public Task<bool> IsTokenValidAsync(string token)
    {
        try
        {
            // Check if token is explicitly blacklisted
            if (_revokedTokens.Contains(token))
                return Task.FromResult(false);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Get user ID from token
            var userIdClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                // Check if user's tokens were revoked after this token was issued
                if (_userTokenRevocationTimes.TryGetValue(userId, out DateTime revocationTime))
                {
                    var issuedAtClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Iat)?.Value;
                    if (long.TryParse(issuedAtClaim, out long issuedAtUnix))
                    {
                        var issuedAt = DateTimeOffset.FromUnixTimeSeconds(issuedAtUnix).UtcDateTime;
                        if (issuedAt < revocationTime)
                            return Task.FromResult(false);
                    }
                }
            }

            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }
}
