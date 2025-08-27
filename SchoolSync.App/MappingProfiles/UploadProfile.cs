using AutoMapper;
using SchoolSync.App.DTOs.Uploads;
using SchoolSync.Domain.Entities;
namespace SchoolSync.App.MappingProfiles;

public class UploadProfile : Profile
{
    public UploadProfile()
    {
        CreateMap<UploadMaterialDto, Material>().ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}

