using System.ComponentModel.DataAnnotations;
namespace SchoolSync.App.DTOs.User;

public class LoginRequestDto
{
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}