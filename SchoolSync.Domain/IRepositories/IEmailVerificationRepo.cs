using SchoolSync.Domain.Entities;
namespace SchoolSync.Domain.IRepositories;

public interface IEmailVerificationRepo
{
    Task<EmailVerification?> GetLatestVerificationAsync(string email);
    Task AddAsync(EmailVerification emailVerification);
    Task MarkVerifiedAsync(string email); // this one uses the first one
}
