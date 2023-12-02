using c_sharp_dotnet_development_lab_3AI_project.database.entities.leaderboard.dto;
using c_sharp_dotnet_development_lab_3AI_project.Services;
using c_sharp_dotnet_development_lab_3AI_project.utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace c_sharp_dotnet_development_lab_3AI_project.Controllers;

[Authorize]
[ApiController]
[Route("/groups/{groupId:guid}/leaderboard")]
public class GroupLeaderboardController : ControllerBase
{
    private readonly IRepository _repository;

    public GroupLeaderboardController(IRepository repository) => _repository = repository;

    [HttpGet("", Name = nameof(GetLeaderboard))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<LeaderboardItemReadDto>> GetLeaderboard(Guid groupId)
    {
        Guid userId = Auth.Jwt.GetUserId(User);
        if (!_repository.UserHasAccessToGroup(userId, groupId))
            return ApiResponse.NotFound;

        return Ok(_repository.GetLeaderboardOfGroup(groupId));
    }
}