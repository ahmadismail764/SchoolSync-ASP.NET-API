using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.Enrollment;

namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/enrollments")]
[Authorize(Roles = "2")]
public class EnrollmentController(IEnrollmentService enrollmentService, IMapper mapper) : ControllerBase
{
    private readonly IEnrollmentService _service = enrollmentService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<EnrollmentDto>>> GetAll()
    {
        var entities = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<EnrollmentDto>>(entities));
    }

    [HttpGet("{id}")]
    public virtual async Task<ActionResult<EnrollmentDto>> GetById(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(_mapper.Map<EnrollmentDto>(entity));
    }

    [HttpPost]
    public virtual async Task<ActionResult<EnrollmentDto>> Create([FromBody] CreateEnrollmentDto dto)
    {
        try
        {
            var entity = _mapper.Map<Enrollment>(dto);
            var created = await _service.CreateAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = (created as dynamic).Id }, _mapper.Map<EnrollmentDto>(created));
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
    public virtual async Task<IActionResult> Update(int id, [FromBody] UpdateEnrollmentDto dto)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        if (dto.StudentId.HasValue) entity.StudentId = dto.StudentId.Value;
        if (dto.SubjectId.HasValue) entity.SubjectId = dto.SubjectId.Value;
        if (dto.TermId.HasValue) entity.TermId = dto.TermId.Value;
        if (dto.Grade.HasValue) entity.Grade = dto.Grade.Value;
        try
        {
            await _service.UpdateAsync(entity);
            return Ok(_mapper.Map<EnrollmentDto>(entity));
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
    public virtual async Task<IActionResult> Delete(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
