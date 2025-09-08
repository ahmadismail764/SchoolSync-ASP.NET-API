using SchoolSync.Domain.Entities;
namespace SchoolSync.Domain.IServices;

public interface ITokenService
{
    string GenerateToken(User user);
    Task RevokeTokensForUserAsync(int userId);
    Task<bool> IsTokenValidAsync(string token);
}
