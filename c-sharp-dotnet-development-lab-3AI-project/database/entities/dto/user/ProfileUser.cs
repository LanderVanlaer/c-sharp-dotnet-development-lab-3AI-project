using AutoMapper;

namespace c_sharp_dotnet_development_lab_3AI_project.database.entities.dto.user;

public class ProfileUser : Profile
{
    public ProfileUser()
    {
        CreateMap<User, UserReadDto>();
        CreateMap<UserWriteDto, User>();
    }
}