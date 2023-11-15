using AutoMapper;

namespace c_sharp_dotnet_development_lab_3AI_project.database.entities.dto.user_group;

public class ProfileUserGroup : Profile
{
    public ProfileUserGroup()
    {
        CreateMap<UserGroup, UserGroupReadDto>();
    }
}