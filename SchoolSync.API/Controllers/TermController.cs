using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.Term;

namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TermController : ControllerBase
{
    private readonly ITermService _service;
    private readonly IMapper _mapper;
    public TermController(ITermService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    // Anyone authenticated (including students) can view terms
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


    [HttpPost]
    public async Task<ActionResult<TermDto>> Create([FromBody] CreateTermDto dto)
    {
        var entity = _mapper.Map<Term>(dto);
        var created = await _service.CreateAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<TermDto>(created));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTermDto dto)
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
}
