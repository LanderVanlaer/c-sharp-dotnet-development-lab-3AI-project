using AutoMapper;

namespace c_sharp_dotnet_development_lab_3AI_project.database.entities.group.dto;

public class ProfileGroup : Profile
{
    public ProfileGroup()
    {
        CreateMap<Group, GroupReadDto>();
        CreateMap<Group, GroupWriteDto>();
    }
}