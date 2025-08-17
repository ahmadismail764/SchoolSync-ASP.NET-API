using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.Organization;

namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrganizationController(IOrganizationService service, IMapper mapper) : ControllerBase
{
    private readonly IOrganizationService _service = service;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrganizationDto>>> GetAll()
    {
        var entities = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<OrganizationDto>>(entities));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrganizationDto>> GetById(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(_mapper.Map<OrganizationDto>(entity));
    }

    [HttpPost]
    public async Task<ActionResult<OrganizationDto>> Create([FromBody] CreateOrganizationDto dto)
    {
        var entity = _mapper.Map<Organization>(dto);
        var created = await _service.CreateAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<OrganizationDto>(created));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateOrganizationDto dto)
    {
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

    [HttpPut("update-range")]
    public async Task<IActionResult> UpdateRange([FromBody] UpdateOrganizationDto dto, [FromQuery] string? nameContains = null)
    {
        var entities = await _service.GetAllAsync();
        var filtered = string.IsNullOrEmpty(nameContains)
            ? entities
            : entities.Where(o => o.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase));
        foreach (var entity in filtered)
        {
            _mapper.Map(dto, entity);
            await _service.UpdateAsync(entity);
        }
        return NoContent();
    }

    [HttpDelete("delete-range")]
    public async Task<IActionResult> DeleteRange([FromQuery] string? nameContains = null)
    {
        var entities = await _service.GetAllAsync();
        var filtered = string.IsNullOrEmpty(nameContains)
            ? entities
            : entities.Where(o => o.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase));
        foreach (var entity in filtered)
        {
            await _service.DeleteAsync(entity.Id);
        }
        return NoContent();
    }
}
