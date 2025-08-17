using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
