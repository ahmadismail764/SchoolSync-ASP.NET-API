using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.User;
using SchoolSync.App.DTOs.Enrollment;
using SchoolSync.App.DTOs.Subject;

namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController
(IUserService service, IEnrollmentService enrollmentService, ISubjectService subjectService, IMapper mapper) : ControllerBase
{
    private readonly IUserService _service = service;
    private readonly IEnrollmentService _enrollmentService = enrollmentService; // for students, mainly
    private readonly ISubjectService subjectService = subjectService; // for teachers
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
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "id");
        var isAdmin = User.IsInRole("Admin");
        if (!isAdmin && (userIdClaim == null || int.Parse(userIdClaim.Value) != id))
            return Forbid();
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
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "id");
        var isAdmin = User.IsInRole("Admin");
        if (!isAdmin && (userIdClaim == null || int.Parse(userIdClaim.Value) != id))
            return Forbid();
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        _mapper.Map(dto, entity);
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
