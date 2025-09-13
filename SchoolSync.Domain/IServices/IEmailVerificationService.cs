using SchoolSync.Domain.Entities;
namespace SchoolSync.Domain.IServices;

public interface IEmailVerificationService
{
    Task MarkVerifiedAsync(string email);

    Task<bool> IsVerifiedAsync(string email);

    Task AddVerificationAsync(EmailVerification emailVerification);

    Task SendVerificationEmailAsync(string email);
}
