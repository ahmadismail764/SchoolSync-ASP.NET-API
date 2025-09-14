using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.Domain.IRepositories;
using Bogus;
using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MimeKit;
namespace SchoolSync.App.Services;

public class EmailVerificationService(IConfiguration config, IEmailVerificationRepo emailVerificationRepo) : IEmailVerificationService
{
    private readonly IEmailVerificationRepo _emailVerificationRepo = emailVerificationRepo;
    private readonly IConfiguration _config = config;

    private async Task SendMailUtility(string email, string tempPassword)
    {
        var smtpHost = _config["Smtp:Host"];
        var smtpPort = int.Parse(_config["Smtp:Port"] ?? "587");
        var smtpUser = _config["Smtp:Username"];
        var smtpPass = _config["Smtp:Password"];
        var fromEmail = _config["Smtp:FromEmail"];
        var fromName = _config["Smtp:FromName"] ?? "SchoolSync";
        var subject = "Email Verification";
        var body = $@"<p>Your verification code is: <strong>{tempPassword}</strong></p>
                <p>This code will expire in 15 minutes.</p>
                <p>If you did not request this, please ignore this email.</p>";

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromName, fromEmail));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(smtpHost, smtpPort, false);
        await client.AuthenticateAsync(smtpUser, smtpPass);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);

    }

    public Task MarkVerifiedAsync(string email) => _emailVerificationRepo.MarkVerifiedAsync(email);

    public async Task<bool> IsVerifiedAsync(string email)
    {
        var currentVerif = await _emailVerificationRepo.GetLatestVerificationAsync(email);
        return currentVerif != null && currentVerif.IsVerified;
    }

    public Task AddVerificationAsync(EmailVerification emailVerification) => _emailVerificationRepo.AddAsync(emailVerification);

    public async Task<bool> CorrectTempPasswordAsync(string email, string tempPassword)
    {
        var currentVerif = await _emailVerificationRepo.GetLatestVerificationAsync(email);
        if (currentVerif == null || currentVerif.IsVerified || currentVerif.ExpiresAt < DateTime.UtcNow)
            return false;
        return currentVerif.TempPassword == tempPassword;
    }

    public async Task<string> SendVerificationEmailAsync(string email)
    {
        string tempPassword = new Faker().Internet.Password(16, false, "[A-Z0-9!@#$%^&*]");
        try
        {
            await SendMailUtility(email, tempPassword);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to send verification email", ex);
        }

        var newVerification = new EmailVerification
        {
            Email = email,
            TempPassword = tempPassword,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15),
            IsVerified = false
        };
        await AddVerificationAsync(newVerification);
        return tempPassword;
    }
}