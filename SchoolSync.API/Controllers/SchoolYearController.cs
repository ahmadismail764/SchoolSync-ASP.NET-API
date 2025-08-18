using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.SchoolYear;

namespace SchoolSync.API.Controllers;

[Authorize(Roles = "2")]
[ApiController]
[Route("api/[controller]")]
// Require authentication for all endpoints in this controller
[Authorize]
public class SchoolYearController(ISchoolYearService service, IMapper mapper) : ControllerBase
{
    private readonly ISchoolYearService _service = service;
    private readonly IMapper _mapper = mapper;

    // Anyone authenticated (including students) can view school years
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SchoolYearDto>>> GetAll()
    {
        var entities = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<SchoolYearDto>>(entities));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SchoolYearDto>> GetById(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(_mapper.Map<SchoolYearDto>(entity));
    }

    [HttpPost]
    public async Task<ActionResult<SchoolYearDto>> Create([FromBody] CreateSchoolYearDto dto)
    {
        var entity = _mapper.Map<SchoolYear>(dto);
        var created = await _service.CreateAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<SchoolYearDto>(created));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSchoolYearDto dto)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();

        if (dto.StartDate.HasValue) entity.StartDate = dto.StartDate.Value;
        if (dto.EndDate.HasValue) entity.EndDate = dto.EndDate.Value;
        if (dto.SchoolId.HasValue) entity.SchoolId = dto.SchoolId.Value;
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
