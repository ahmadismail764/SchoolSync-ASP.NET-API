using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.SchoolYear;

namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchoolYearController(ISchoolYearService service, IMapper mapper)
    : GenericController<SchoolYear, SchoolYearDto, CreateSchoolYearDto, UpdateSchoolYearDto>(service, mapper)
{ }
