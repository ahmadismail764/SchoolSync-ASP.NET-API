using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.Lesson;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/lesson")]
[Authorize(Roles = "2")]
public class LessonController(ILessonService<Lesson> lessonService, IMapper mapper) : ControllerBase
{
    private readonly ILessonService<Lesson> _lessonService = lessonService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LessonDto>>> GetAll()
    {
        var lessons = await _lessonService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<LessonDto>>(lessons));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LessonDto>> GetById(int id)
    {
        var lesson = await _lessonService.GetByIdAsync(id);
        if (lesson == null) return NotFound();
        return Ok(_mapper.Map<LessonDto>(lesson));
    }

    [HttpGet("by-subject/{subjectId}")]
    public async Task<ActionResult<IEnumerable<LessonDto>>> GetBySubject(int subjectId)
    {
        var lessons = await _lessonService.GetBySubjectAsync(subjectId);
        return Ok(_mapper.Map<IEnumerable<LessonDto>>(lessons));
    }

    [HttpPost]
    public async Task<ActionResult<LessonDto>> Create([FromBody] CreateLessonDto dto)
    {
        var lesson = _mapper.Map<Lesson>(dto);
        var created = await _lessonService.CreateAsync(lesson);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<LessonDto>(created));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<LessonDto>> Update(int id, [FromBody] UpdateLessonDto dto)
    {
        var lesson = await _lessonService.GetByIdAsync(id);
        if (lesson == null) return NotFound();
        _mapper.Map(dto, lesson);
        await _lessonService.UpdateAsync(lesson);
        return Ok(_mapper.Map<LessonDto>(lesson));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var lesson = await _lessonService.GetByIdAsync(id);
        if (lesson == null) return NotFound();
        await _lessonService.DeleteAsync(id);
        return NoContent();
    }
}
