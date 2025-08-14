using AutoMapper;
using SchoolSync.Domain.Entities;
using SchoolSync.App.DTOs.SchoolYear;

namespace SchoolSync.App.MappingProfiles;

public class SchoolYearProfile : Profile
{
    public SchoolYearProfile()
    {
        CreateMap<SchoolYear, SchoolYearDto>().ReverseMap();
        CreateMap<SchoolYear, CreateSchoolYearDto>().ReverseMap();
        CreateMap<SchoolYear, UpdateSchoolYearDto>().ReverseMap();
    }
}
