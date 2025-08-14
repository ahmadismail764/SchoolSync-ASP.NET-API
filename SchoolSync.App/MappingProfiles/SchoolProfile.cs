using AutoMapper;
using SchoolSync.Domain.Entities;
using SchoolSync.App.DTOs.School;

namespace SchoolSync.App.MappingProfiles;

public class SchoolProfile : Profile
{
    public SchoolProfile()
    {
        CreateMap<School, SchoolDto>().ReverseMap();
        CreateMap<School, CreateSchoolDto>().ReverseMap();
        CreateMap<School, UpdateSchoolDto>().ReverseMap();
    }
}
