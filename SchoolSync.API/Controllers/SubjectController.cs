using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.Subject;

namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectController(ISubjectService service, IMapper mapper)
    : GenericController<Subject, SubjectDto, CreateSubjectDto, UpdateSubjectDto>(service, mapper)
{ }
