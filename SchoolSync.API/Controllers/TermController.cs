using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolSync.Domain.Entities;
using SchoolSync.Domain.IServices;
using SchoolSync.App.DTOs.Term;

namespace SchoolSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TermController(ITermService service, IMapper mapper)
    : GenericController<Term, TermDto, CreateTermDto, UpdateTermDto>(service, mapper)
{ }
