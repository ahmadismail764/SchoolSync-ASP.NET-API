
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

    [Authorize(Roles = "2")]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        // Debug: Check and return the user's role claim
        var roleClaim = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
        var allClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(new
        {
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
        _mapper.Map(dto, entity);
        try
        {
            await _service.UpdateAsync(entity);
            return Ok(_mapper.Map<UserDto>(entity));
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
