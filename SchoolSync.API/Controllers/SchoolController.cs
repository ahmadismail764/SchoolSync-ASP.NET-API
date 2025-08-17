using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.School;

namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class SchoolController(ISchoolService service, IMapper mapper) : ControllerBase
{
    private readonly ISchoolService _service = service;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SchoolDto>>> GetAll()
    {
        var entities = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<SchoolDto>>(entities));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SchoolDto>> GetById(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(_mapper.Map<SchoolDto>(entity));
    }

    [HttpPost]
    public async Task<ActionResult<SchoolDto>> Create([FromBody] CreateSchoolDto dto)
    {
        var entity = _mapper.Map<School>(dto);
        var created = await _service.CreateAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<SchoolDto>(created));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSchoolDto dto)
    {
    var entity = await _service.GetByIdAsync(id);
    if (entity == null) return NotFound();

    if (dto.Name != null) entity.Name = dto.Name;
    if (dto.Address != null) entity.Address = dto.Address;
    if (dto.OrganizationId.HasValue) entity.OrganizationId = dto.OrganizationId.Value;
    if (dto.IsActive.HasValue) entity.IsActive = dto.IsActive.Value;

    await _service.UpdateAsync(entity);
    return NoContent();
    }

    [HttpPut("range")]
    public async Task<IActionResult> UpdateRange([FromBody] UpdateSchoolDto dto, [FromQuery] string? nameContains = null)
    {
        var entities = await _service.GetAllAsync();
        var filtered = string.IsNullOrEmpty(nameContains)
            ? entities
            : entities.Where(s => s.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase));
        foreach (var entity in filtered)
        {
            _mapper.Map(dto, entity);
            await _service.UpdateAsync(entity);
        }
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


    [HttpDelete("range")]
    public async Task<IActionResult> DeleteRange([FromQuery] string? nameContains = null)
    {
        var entities = await _service.GetAllAsync();
        var filtered = string.IsNullOrEmpty(nameContains)
            ? entities
            : entities.Where(s => s.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase));
        foreach (var entity in filtered)
        {
            await _service.DeleteAsync(entity.Id);
        }
        return NoContent();
    }
}
