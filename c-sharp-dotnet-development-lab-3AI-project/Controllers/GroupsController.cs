using AutoMapper;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.group;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.group.dto;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.user;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.user_group;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.user.dto;
using c_sharp_dotnet_development_lab_3AI_project.Services;
using c_sharp_dotnet_development_lab_3AI_project.utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace c_sharp_dotnet_development_lab_3AI_project.Controllers;

[Authorize]
[ApiController]
[Route("/groups")]
public class GroupsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRepository _repository;

    public GroupsController(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<GroupReadDto>> GetAllUsersGroups() =>
        Ok(_mapper.Map<IEnumerable<GroupReadDto>>(_repository.GetAllGroupsOfUser(Auth.Jwt.GetUserId(User))));

    [HttpGet("{id:guid}", Name = nameof(GetGroup))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<GroupReadDto> GetGroup(Guid id)
    {
        Group? group = _repository.GetGroupWithUsersGroups(id);

        if (group == null)
            return ApiResponse.NotFound;

        Guid userId = Auth.Jwt.GetUserId(User);
        return group.UserGroups.All(userGroup => userGroup.UserId != userId)
            ? ApiResponse.NotFound
            : Ok(_mapper.Map<GroupReadDto>(group));
    }

    [HttpGet("{id:guid}/users", Name = nameof(GetGroupUsers))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<UserReadDto>> GetGroupUsers(Guid id)
    {
        if (!_repository.UserHasAccessToGroup(Auth.Jwt.GetUserId(User), id))
            return ApiResponse.NotFound;

        IEnumerable<User> users = _repository.GetUsersByGroupId(id);
        return Ok(_mapper.Map<IEnumerable<UserReadDto>>(users));
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> AddGroup(GroupWriteDto dto)
    {
        Group group = _mapper.Map<Group>(dto);
        _repository.AddGroup(group);

        UserGroup userGroup = new()
        {
            GroupId = group.Id,
            UserId = Auth.Jwt.GetUserId(User),
        };
        _repository.AddUserGroup(userGroup);

        await _repository.SaveChanges();

        return CreatedAtRoute(nameof(GetGroup), new { group.Id }, _mapper.Map<GroupReadDto>(group));
    }
}