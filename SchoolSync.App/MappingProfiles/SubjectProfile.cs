using AutoMapper;
using SchoolSync.Domain.Entities;
using SchoolSync.App.DTOs.Subject;

namespace SchoolSync.App.MappingProfiles;

public class SubjectProfile : Profile
{
    public SubjectProfile()
    {
        CreateMap<Subject, SubjectDto>().ReverseMap();
        CreateMap<Subject, CreateSubjectDto>().ReverseMap();
        CreateMap<Subject, UpdateSubjectDto>().ReverseMap();
    }
}
