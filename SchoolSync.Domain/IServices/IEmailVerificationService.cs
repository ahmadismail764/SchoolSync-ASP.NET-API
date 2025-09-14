using SchoolSync.Domain.Entities;
namespace SchoolSync.Domain.IServices;

public interface IEmailVerificationService
{
    Task MarkVerifiedAsync(string email);

    Task<bool> CorrectTempPasswordAsync(string EMail, string tempPassword);

    Task<bool> IsVerifiedAsync(string email);

    Task AddVerificationAsync(EmailVerification emailVerification);

    Task<string> SendVerificationEmailAsync(string email);
}
