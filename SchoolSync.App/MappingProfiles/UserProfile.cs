using AutoMapper;
using SchoolSync.Domain.Entities;
using SchoolSync.App.DTOs.User;
using SchoolSync.App.DTOs.Subject;

namespace SchoolSync.App.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Subjects, opt => opt.MapFrom((src, _, _, context) =>
            {
                // If student, get subjects from enrollments
                if (src.Role?.Name == "Student")
                    return src.Enrollments?.Where(e => e.Subject != null).Select(e => context.Mapper.Map<SubjectDto>(e.Subject)).ToList() ?? [];
                
                // If teacher, get subjects they teach
                if (src.Role?.Name == "Teacher")
                    return src.Subjects?.Select(s => context.Mapper.Map<SubjectDto>(s)).ToList() ?? [];
                return [];
            }))
            .ReverseMap();
        CreateMap<User, CreateUserDto>().ReverseMap();
        CreateMap<User, UpdateUserDto>().ReverseMap();
    }
}
