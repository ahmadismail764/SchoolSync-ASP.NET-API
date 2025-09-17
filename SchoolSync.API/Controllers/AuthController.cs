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
            var user = await _userService.AuthenticateAsync(request.Email, request.Password);
            bool verif_status = await _emailVerifService.IsVerifiedAsync(request.Email);

            if (verif_status)
            {
                var token = _tokenService.GenerateToken(user);
                return Ok(new { token });
            }
            else
            {
                // Require password change without marking as verified yet
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                user.PasswordHash = string.Empty;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                await _userService.UpdateAsync(user);
                return Ok(new
                {
                    requirePasswordChange = true,
                    message = "Please set a new password",
                    redirectTo = "/api/auth/set-password"
                });
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
    [AllowAnonymous]
    public async Task<ActionResult> SetNewPassword([FromBody] SetPasswordDto dto)
    {
        try
        {
            var user = await _userService.GetByEmailAsync(dto.Email);

            var isValidPassword = await _emailVerifService.CorrectTempPasswordAsync(dto.Email, dto.OldPassword);
            if (!isValidPassword) return Unauthorized("Invalid current password.");

            if (dto.NewPassword != dto.ConfirmNewPassword)
                return BadRequest("Passwords do not match.");

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.NewPassword);
            await _userService.UpdateAsync(user);

            // Only mark as verified if not already verified (for first-time password setup)
            bool isAlreadyVerified = await _emailVerifService.IsVerifiedAsync(dto.Email);
            if (!isAlreadyVerified)
            {
                await _emailVerifService.MarkVerifiedAsync(dto.Email);
            }

            return Ok(new
            {
                message = "Password updated successfully. Please login with your new password.",
                redirectTo = "/api/auth/login"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
