using SchoolSync.Domain.Entities;
namespace SchoolSync.Domain.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}
