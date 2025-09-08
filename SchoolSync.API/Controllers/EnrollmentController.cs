using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.Enrollment;

namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/enrollments")]
[Authorize(Roles = "Admin,Teacher")]  // Admin and Teacher
public class EnrollmentController(IEnrollmentService enrollmentService) : ControllerBase
{
    private readonly IEnrollmentService _service = enrollmentService;

    // Helper methods for manual mapping
    private EnrollmentDto MapToDto(Enrollment enrollment)
    {
        return new EnrollmentDto
        {
            UserId = enrollment.StudentId,
            TermId = enrollment.TermId,
            SubjectId = enrollment.SubjectId,
            Grade = enrollment.Grade
        };
    }

    private static Enrollment MapToEntity(CreateEnrollmentDto dto)
    {
        return new Enrollment
        {
            StudentId = dto.StudentId,
            SubjectId = dto.SubjectId,
            TermId = dto.TermId
        };
    }

    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<EnrollmentDto>>> GetAll()
    {
        var entities = await _service.GetAllAsync();
        var dtos = entities.Select(MapToDto);
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public virtual async Task<ActionResult<EnrollmentDto>> GetById(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(MapToDto(entity));
    }

    [HttpPost]
    public virtual async Task<ActionResult<EnrollmentDto>> Create([FromBody] CreateEnrollmentDto dto)
    {
        var entity = MapToEntity(dto);
        var created = await _service.CreateAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapToDto(created));
    }


    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(int id, [FromBody] UpdateEnrollmentDto dto)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();

        // Manual mapping - only update Grade if provided
        if (dto.Grade.HasValue)
        {
            entity.Grade = dto.Grade.Value;
        }

        await _service.UpdateAsync(entity);
        return Ok(MapToDto(entity));
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
