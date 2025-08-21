using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.Subject;

namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "2")]
public class SubjectController(ISubjectService service, IEnrollmentService enrollmentService, IMapper mapper) : ControllerBase
{
    private readonly ISubjectService _service = service;
    private readonly IEnrollmentService _enrollmentService = enrollmentService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubjectDto>>> GetAll()
    {
        var entities = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<SubjectDto>>(entities));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SubjectDto>> GetById(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(_mapper.Map<SubjectDto>(entity));
    }
    [HttpGet("range")]
    public async Task<ActionResult<IEnumerable<SubjectDto>>> GetRange([FromQuery] string? nameContains = null)
    {
        var entities = await _service.GetRangeWhereAsync(
            sub => string.IsNullOrEmpty(nameContains) || sub.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase)
        );
        return Ok(_mapper.Map<IEnumerable<SubjectDto>>(entities));
    }


    [HttpPost]
    public async Task<ActionResult<SubjectDto>> Create([FromBody] CreateSubjectDto dto)
    {
        try
        {
            var entity = _mapper.Map<Subject>(dto);
            var created = await _service.CreateAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<SubjectDto>(created));
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

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSubjectDto dto)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        // Explicit guard clause pattern for partial update
        if (dto.Name != null) entity.Name = dto.Name;
        if (dto.Code != null) entity.Code = dto.Code;
        if (dto.Credits.HasValue) entity.Credits = dto.Credits.Value;
        if (dto.SchoolId.HasValue) entity.SchoolId = dto.SchoolId.Value;
        if (dto.TeacherId.HasValue) entity.TeacherId = dto.TeacherId.Value;
        if (dto.IsActive.HasValue) entity.IsActive = dto.IsActive.Value;
        try
        {
            await _service.UpdateAsync(entity);
            return Ok(_mapper.Map<SubjectDto>(entity));
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        await _service.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("my-enrolled/{studentId}")]
    public async Task<ActionResult<IEnumerable<SubjectDto>>> GetEnrolledSubjects(int studentId)
    {
        var enrollments = await _enrollmentService.GetByStudentAsync(studentId);
        var subjects = enrollments.Select(e => e.Subject).Where(s => s != null);
        var subjectDtos = subjects.Select(s => _mapper.Map<SubjectDto>(s));
        return Ok(subjectDtos);
    }
    [HttpPut("range")]
    public async Task<ActionResult<IEnumerable<SubjectDto>>> UpdateRange([FromBody] UpdateSubjectDto dto, [FromQuery] string? nameContains = null)
    {
        var entity = new Subject();
        _mapper.Map(dto, entity);
        try
        {
            var updated = await _service.UpdateRangeWhereAsync(
                sub => string.IsNullOrEmpty(nameContains) || sub.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase),
                entity
            );
            return Ok(_mapper.Map<IEnumerable<SubjectDto>>(updated));
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
    public async Task<ActionResult<IEnumerable<SubjectDto>>> DeleteRange([FromQuery] string? nameContains = null)
    {
        try
        {
            var deleted = await _service.DeleteRangeWhereAsync(sub => string.IsNullOrEmpty(nameContains) || sub.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase));
            return Ok(_mapper.Map<IEnumerable<SubjectDto>>(deleted));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}
