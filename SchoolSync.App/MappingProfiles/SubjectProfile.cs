using AutoMapper;
using SchoolSync.Domain.Entities;
using SchoolSync.App.DTOs.Subject;

namespace SchoolSync.App.MappingProfiles;

public class SubjectProfile : Profile
{
    public SubjectProfile()
    {
        CreateMap<Subject, SubjectDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.Credits, opt => opt.MapFrom(src => src.Credits))
            .ForMember(dest => dest.SchoolId, opt => opt.MapFrom(src => src.SchoolId))
            .ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.TeacherId));

        CreateMap<SubjectDto, Subject>()
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        CreateMap<Subject, CreateSubjectDto>().ReverseMap();
        CreateMap<Subject, UpdateSubjectDto>().ReverseMap();
    }
}
