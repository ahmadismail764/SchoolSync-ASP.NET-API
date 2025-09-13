using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IRepositories;
using SchoolSync.Infra.Persistence;
using Microsoft.EntityFrameworkCore;
namespace SchoolSync.Infra.Repositories;

public class EmailVerificationRepo(DBContext context) : IEmailVerificationRepo
{
    private readonly DBContext _context = context;
    public Task<EmailVerification?> GetLatestVerificationAsync(string email)
    {
        return _context.EmailVerifications
            .Where(ev => ev.Email == email)
            .OrderByDescending(ev => ev.ExpiresAt)
            .FirstOrDefaultAsync();
    }
    public async Task AddAsync(EmailVerification emailVerification)
    {
        _context.EmailVerifications.Add(emailVerification);
        await _context.SaveChangesAsync();
    }
    public async Task MarkVerifiedAsync(string email)
    {
        var verification = await GetLatestVerificationAsync(email);
        if (verification != null)
        {
            verification.IsVerified = true;
            await _context.SaveChangesAsync();
        }
    }
}
