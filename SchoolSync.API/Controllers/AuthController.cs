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
public class AuthController(IUserService userService, ITokenService tokenService, IMapper mapper, IPasswordHasher<User> passwordHasher) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IMapper _mapper = mapper;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userService.AuthenticateAsync(request.Username, request.Password);
        if (user == null) return Unauthorized();

        var token = _tokenService.GenerateToken(user);
        return Ok(new { token });
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> Register([FromBody] CreateUserDto dto)
    {
        var entity = _mapper.Map<User>(dto);
        if (string.IsNullOrWhiteSpace(dto.Password) || !dto.Password.All(char.IsLetterOrDigit))
            return BadRequest("Password must be alphanumeric.");
        entity.PasswordHash = _passwordHasher.HashPassword(entity, dto.Password);
        var created = await _userService.CreateAsync(entity);
        return CreatedAtAction(nameof(Register), new { id = created.Id }, _mapper.Map<UserDto>(created));
    }
}

public class LoginRequest
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}