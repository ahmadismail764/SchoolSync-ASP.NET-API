
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.User;
namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "2")]
public class UserController
(IUserService service, IMapper mapper) : ControllerBase
{
    private readonly IUserService _service = service;
    private readonly IMapper _mapper = mapper;

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<UserDto>> Register([FromBody] CreateUserDto dto)
    {
        try
        {
            var entity = _mapper.Map<User>(dto);
            if (!string.IsNullOrEmpty(dto.Password))
                entity.PasswordHash = dto.Password;
            var created = await _service.CreateAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<UserDto>(created));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();

        return Ok(_mapper.Map<UserDto>(entity));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var entities = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<UserDto>>(entities));
    }

    [HttpGet("range")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetRange([FromQuery] string? nameContains = null)
    {
        var entities = await _service.GetRangeWhereAsync(
            user => string.IsNullOrEmpty(nameContains) || user.FullName.Contains(nameContains, StringComparison.OrdinalIgnoreCase)
        );
        return Ok(_mapper.Map<IEnumerable<UserDto>>(entities));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        // Only update fields that are not null in the DTO
        if (dto.FullName != null) entity.FullName = dto.FullName;
        if (dto.Email != null) entity.Email = dto.Email;
        if (dto.Username != null) entity.Username = dto.Username;
        if (dto.SchoolId.HasValue) entity.SchoolId = dto.SchoolId.Value;
        if (dto.RoleId.HasValue) entity.RoleId = dto.RoleId.Value;
        if (dto.IsActive.HasValue) entity.IsActive = dto.IsActive.Value;
        try
        {
            await _service.UpdateAsync(entity);
            var pass = _mapper.Map<UserDto>(entity);
            return Ok(pass);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpPut("range")]
    public async Task<ActionResult<IEnumerable<UserDto>>> UpdateRange([FromBody] UpdateUserDto dto, [FromQuery] string? nameContains = null)
    {
        var entity = new User();
        _mapper.Map(dto, entity);
        try
        {
            var updated = await _service.UpdateRangeWhereAsync(
                user => string.IsNullOrEmpty(nameContains) || user.FullName.Contains(nameContains, StringComparison.OrdinalIgnoreCase),
                entity
            );
            return Ok(_mapper.Map<IEnumerable<UserDto>>(updated));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpDelete("range")]
    public async Task<ActionResult<IEnumerable<UserDto>>> DeleteRange([FromQuery] string? nameContains = null)
    {
        try
        {
            var deleted = await _service.DeleteRangeWhereAsync(user => string.IsNullOrEmpty(nameContains) || user.FullName.Contains(nameContains, StringComparison.OrdinalIgnoreCase));
            return Ok(_mapper.Map<IEnumerable<UserDto>>(deleted));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        await _service.DeleteAsync(id);
        return NoContent();
    }

}
