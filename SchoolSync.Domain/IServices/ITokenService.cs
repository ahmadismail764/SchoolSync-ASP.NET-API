using SchoolSync.Domain.Entities;
namespace SchoolSync.Domain.IServices;

public interface ITokenService
{
    string GenerateToken(User user);
}
