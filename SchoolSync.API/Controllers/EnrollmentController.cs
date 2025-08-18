using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.Enrollment;

namespace SchoolSync.API.Controllers;

[Authorize(Roles = "2")]
[ApiController]
[Route("api/[controller]")]
// Require authentication for all endpoints in this controller
[Authorize]
public class EnrollmentController(IEnrollmentService enrollmentService, IMapper mapper) : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService = enrollmentService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<EnrollmentDto>>> GetAll()
    {
        var entities = await _enrollmentService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<EnrollmentDto>>(entities));
    }

    [HttpGet("{id}")]
    public virtual async Task<ActionResult<EnrollmentDto>> GetById(int id)
    {
        var entity = await _enrollmentService.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(_mapper.Map<EnrollmentDto>(entity));
    }

    [HttpPost]
    public virtual async Task<ActionResult<EnrollmentDto>> Create([FromBody] CreateEnrollmentDto dto)
    {
        var entity = _mapper.Map<Enrollment>(dto);
        var created = await _enrollmentService.CreateAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = (created as dynamic).Id }, _mapper.Map<EnrollmentDto>(created));
    }


    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(int id, [FromBody] UpdateEnrollmentDto dto)
    {
    var entity = await _enrollmentService.GetByIdAsync(id);
    if (entity == null) return NotFound();

    if (dto.StudentId.HasValue) entity.StudentId = dto.StudentId.Value;
    if (dto.SubjectId.HasValue) entity.SubjectId = dto.SubjectId.Value;
    if (dto.TermId.HasValue) entity.TermId = dto.TermId.Value;
    if (dto.EnrollmentDate.HasValue) entity.EnrollmentDate = dto.EnrollmentDate.Value;
    if (dto.Grade.HasValue) entity.Grade = dto.Grade.Value;
    if (dto.IsActive.HasValue) entity.IsActive = dto.IsActive.Value;

    await _enrollmentService.UpdateAsync(entity);
    return NoContent();
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(int id)
    {
        var entity = await _enrollmentService.GetByIdAsync(id);
        if (entity == null) return NotFound();
        await _enrollmentService.DeleteAsync(id);
        return NoContent();
    }
 }
