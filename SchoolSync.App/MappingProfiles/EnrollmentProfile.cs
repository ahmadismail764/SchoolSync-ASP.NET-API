using AutoMapper;
using SchoolSync.Domain.Entities;
using SchoolSync.App.DTOs.Enrollment;

namespace SchoolSync.App.MappingProfiles;

public class EnrollmentProfile : Profile
{
    public EnrollmentProfile()
    {
        CreateMap<Enrollment, EnrollmentDto>().ReverseMap();
        CreateMap<Enrollment, CreateEnrollmentDto>().ReverseMap();
        CreateMap<Enrollment, UpdateEnrollmentDto>().ReverseMap();
    }
}
