using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.Subject;

namespace SchoolSync.API.Controllers;

[Authorize(Roles = "2")]
[ApiController]
[Route("api/[controller]")]
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
        var entity = _mapper.Map<Subject>(dto);
        var created = await _service.CreateAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<SubjectDto>(created));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSubjectDto dto)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        _mapper.Map(dto, entity);
        await _service.UpdateAsync(entity);
        try
        {
            return NoContent();
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
    public async Task<IActionResult> UpdateRange([FromBody] UpdateSubjectDto dto, [FromQuery] string? nameContains = null)
    {
        var entity = new Subject();
        _mapper.Map(dto, entity);
        await _service.UpdateRangeWhereAsync(
            sub => string.IsNullOrEmpty(nameContains) || sub.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase),
            entity
        );
        return NoContent();
    }

    [HttpDelete("range")]
    public async Task<IActionResult> DeleteRange([FromQuery] string? nameContains = null)
    {
        try
        {
            await _service.DeleteRangeWhereAsync(sub => string.IsNullOrEmpty(nameContains) || sub.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
        return NoContent();
    }
}
