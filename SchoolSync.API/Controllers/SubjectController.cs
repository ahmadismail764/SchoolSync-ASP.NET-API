using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.Subject;

namespace SchoolSync.API.Controllers;

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

    [Authorize(Roles = "Teacher")]
    [HttpPost]
    public async Task<ActionResult<SubjectDto>> Create([FromBody] CreateSubjectDto dto)
    {
        var entity = _mapper.Map<Subject>(dto);
        var created = await _service.CreateAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<SubjectDto>(created));
    }

    [Authorize(Roles = "Teacher")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSubjectDto dto)
    {
    var entity = await _service.GetByIdAsync(id);
    if (entity == null) return NotFound();

    if (dto.Name != null) entity.Name = dto.Name;
    if (dto.Code != null) entity.Code = dto.Code;
    if (dto.Credits.HasValue) entity.Credits = dto.Credits.Value;
    if (dto.SchoolId.HasValue) entity.SchoolId = dto.SchoolId.Value;
    if (dto.TeacherId.HasValue) entity.TeacherId = dto.TeacherId.Value;
    if (dto.IsActive.HasValue) entity.IsActive = dto.IsActive.Value;

    await _service.UpdateAsync(entity);
    return NoContent();
    }

    [Authorize(Roles = "Teacher")]
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
}
