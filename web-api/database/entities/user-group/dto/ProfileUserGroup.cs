using AutoMapper;

namespace c_sharp_dotnet_development_lab_3AI_project.database.entities.user_group.dto;

public class ProfileUserGroup : Profile
{
    public ProfileUserGroup()
    {
        CreateMap<UserGroup, UserGroupReadDto>();
    }
}