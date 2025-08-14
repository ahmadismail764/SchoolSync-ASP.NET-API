using AutoMapper;
using SchoolSync.Domain.Entities;
using SchoolSync.App.DTOs.Term;

namespace SchoolSync.App.MappingProfiles;

public class TermProfile : Profile
{
    public TermProfile()
    {
        CreateMap<Term, TermDto>().ReverseMap();
        CreateMap<Term, CreateTermDto>().ReverseMap();
        CreateMap<Term, UpdateTermDto>().ReverseMap();
    }
}
