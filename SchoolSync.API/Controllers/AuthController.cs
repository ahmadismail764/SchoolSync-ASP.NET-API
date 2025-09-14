using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.App.DTOs.User;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IUserService userService, IEmailVerificationService emailVerifService, ITokenService tokenService, IMapper mapper, IPasswordHasher<User> passwordHasher) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IEmailVerificationService _emailVerifService = emailVerifService;
    private readonly IMapper _mapper = mapper;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            bool verif_status = await _emailVerifService.IsVerifiedAsync(request.Email);
            var user = await _userService.AuthenticateAsync(request.Email, request.Password);
            if (verif_status)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                // This is already handled by the user service's AuthenticateAsync method.
                // So it is impossible to return null here.
                var token = _tokenService.GenerateToken(user);
#pragma warning restore CS8604 // Possible null reference argument.
                return Ok(new { token });
            }
            else
            {
                await _emailVerifService.MarkVerifiedAsync(request.Email);
                return RedirectToAction(nameof(SetNewPassword), new { message = "Email verified successfully. Please set your new password." });
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult> Register([FromBody] CreateUserDto dto)
    {
        try
        {
            var tempPassword = await _emailVerifService.SendVerificationEmailAsync(dto.Email);
            var entity = _mapper.Map<User>(dto);
            entity.PasswordHash = _passwordHasher.HashPassword(entity, tempPassword);
            var created = await _userService.CreateAsync(entity);
            return Ok(new
            {
                message = "User created successfully.",
                redirectTo = "/api/auth/login",
                userId = created.Id
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("set-password")]
    public async Task<ActionResult> SetNewPassword([FromBody] SetPasswordDto dto)
    {
        try
        {
            var user = await _userService.AuthenticateAsync(dto.Email, dto.OldPassword);
            if (dto.NewPassword != dto.ConfirmNewPassword)
                return BadRequest("Passwords do not match.");

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            // This is already handled by the user service's AuthenticateAsync method.
            // So it is impossible to return null here.
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.NewPassword);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            await _userService.UpdateAsync(user);
            // Redirect to login with success message
            return RedirectToAction(nameof(Login), new { message = "Password updated successfully. Please login with your new password." });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
