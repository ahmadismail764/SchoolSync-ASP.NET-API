using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.Term;

namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TermController(ITermService service, IMapper mapper) : ControllerBase
{
    private readonly ITermService _service = service;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TermDto>>> GetAll()
    {
        var entities = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<TermDto>>(entities));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TermDto>> GetById(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();
        return Ok(_mapper.Map<TermDto>(entity));
    }


    [Authorize(Roles = "Teacher")]
    [HttpPost]
    public async Task<ActionResult<TermDto>> Create([FromBody] CreateTermDto dto)
    {
        var entity = _mapper.Map<Term>(dto);
        var created = await _service.CreateAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<TermDto>(created));
    }

    [Authorize(Roles = "Teacher")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTermDto dto)
    {
    var entity = await _service.GetByIdAsync(id);
    if (entity == null) return NotFound();

    if (dto.Name != null) entity.Name = dto.Name;
    if (dto.StartDate.HasValue) entity.StartDate = dto.StartDate.Value;
    if (dto.EndDate.HasValue) entity.EndDate = dto.EndDate.Value;
    if (dto.SchoolYearId.HasValue) entity.SchoolYearId = dto.SchoolYearId.Value;
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
}
