using SchoolSync.Domain.Entities;
namespace SchoolSync.App.Services;
// Interface for the token service, responsible for generating JWTs for users
public interface ITokenService
{
    // Generates a JWT token for the given user
    string GenerateToken(User user);
}
