using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.IServices;
using SchoolSync.Domain.Entities;
using SchoolSync.App.Services;

namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService, ITokenService tokenService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await userService.GetByUsernameAsync(request.Username);
        if (user == null || !await userService.ValidatePasswordAsync(user, request.Password))
            return Unauthorized();

        var token = tokenService.GenerateToken(user);
        return Ok(new { token });
    }
}

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
