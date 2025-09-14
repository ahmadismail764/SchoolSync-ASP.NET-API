using System.ComponentModel.DataAnnotations;
namespace SchoolSync.App.DTOs.User;

public class SetPasswordDto
{
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string OldPassword { get; set; } = null!;
    [Required]
    public string NewPassword { get; set; } = null!;
    [Required]
    public string ConfirmNewPassword { get; set; } = null!;
}