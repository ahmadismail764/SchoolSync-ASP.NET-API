namespace SchoolSync.Domain.Entities;

public class EmailVerification
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string TempPassword { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public bool IsVerified { get; set; } = false;
}