using AutoMapper;
using SchoolSync.Domain.Entities;
using SchoolSync.App.DTOs.Material;

namespace SchoolSync.App.MappingProfiles;

public class MaterialProfile : Profile
{
    public MaterialProfile()
    {
        CreateMap<CreateMaterialDto, Material>();
        CreateMap<UpdateMaterialDto, Material>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
