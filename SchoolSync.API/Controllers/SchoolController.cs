using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.School;

namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchoolController(ISchoolService service, IMapper mapper)
    : GenericController<School, SchoolDto, CreateSchoolDto, UpdateSchoolDto>(service, mapper)
{ }
