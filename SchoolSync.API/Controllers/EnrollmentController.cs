using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.Enrollment;

namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
        _mapper.Map(dto, entity);
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
