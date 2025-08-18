
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.User;

namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController
(IUserService service, IMapper mapper) : ControllerBase
{
    private readonly IUserService _service = service;
    private readonly IMapper _mapper = mapper;

    [HttpPost]
    public async Task<ActionResult<UserDto>> Register([FromBody] CreateUserDto dto)
    {
        var entity = _mapper.Map<User>(dto);
        if (!string.IsNullOrEmpty(dto.Password))
            entity.PasswordHash = dto.Password;
        var created = await _service.CreateAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<UserDto>(created));
    }

    [Authorize(Roles = "2")]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        // Debug: Check and return the user's role claim
        var roleClaim = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
        var allClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(new {
            User = _mapper.Map<UserDto>(entity),
            RoleClaim = roleClaim,
            AllClaims = allClaims
        });
    }

    [Authorize(Roles = "2")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var entities = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<UserDto>>(entities));
    }

    [Authorize(Roles = "2")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();

        if (dto.FullName != null) entity.FullName = dto.FullName;
        if (dto.Email != null) entity.Email = dto.Email;
        if (dto.Username != null) entity.Username = dto.Username;
        if (dto.SchoolId.HasValue) entity.SchoolId = dto.SchoolId.Value;
        if (dto.RoleId.HasValue) entity.RoleId = dto.RoleId.Value;
        if (dto.IsActive.HasValue) entity.IsActive = dto.IsActive.Value;

        await _service.UpdateAsync(entity);
        return NoContent();
    }

    [Authorize(Roles = "2")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
