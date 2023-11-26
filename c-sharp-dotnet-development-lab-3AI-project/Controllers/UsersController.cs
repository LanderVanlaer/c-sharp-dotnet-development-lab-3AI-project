using System.Net;
using AutoMapper;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.user;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.user.dto;
using c_sharp_dotnet_development_lab_3AI_project.Services;
using c_sharp_dotnet_development_lab_3AI_project.Services.MySql;
using c_sharp_dotnet_development_lab_3AI_project.utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace c_sharp_dotnet_development_lab_3AI_project.Controllers;

[Authorize]
[ApiController]
[Route("/users")]
public class UsersController : ControllerBase
{
    private static readonly ActionResult UserAlreadyExistsError =
        ApiResponse.Create(HttpStatusCode.Conflict, "User already exists.");

    private readonly IMapper _mapper;
    private readonly IRepository _repository;

    public UsersController(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("me", Name = nameof(GetUser))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<UserReadDto> GetUser()
    {
        User? user = _repository.GetUser(Auth.Jwt.GetUserId(User));

        if (user == null) throw new Exception("User not found. While using [Authorize] attribute.");

        return Ok(_mapper.Map<UserReadDto>(user));
    }

    [AllowAnonymous]
    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> AddUser(UserWriteDto dto)
    {
        User user = _mapper.Map<User>(dto);

        user.PasswordHash = new PasswordHasher<User>().HashPassword(user, dto.Password);

        _repository.AddUser(user);
        try
        {
            await _repository.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            if (CustomMySqlHelper.IsMySqlException(e, MySqlExceptionType.Duplicate))
                return UserAlreadyExistsError;

            throw;
        }

        return CreatedAtRoute(nameof(GetUser), new { user.Id }, _mapper.Map<UserReadDto>(user));
    }

    [HttpPatch("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> UpdateUser(UserUpdateDto dto)
    {
        User? user = _repository.GetUser(Auth.Jwt.GetUserId(User));
        if (user == null) throw new Exception("User not found. While using [Authorize] attribute.");

        if (dto.Username is not null) user.Username = dto.Username;
        if (dto.Password is not null) user.PasswordHash = new PasswordHasher<User>().HashPassword(user, dto.Password);

        try
        {
            await _repository.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            if (CustomMySqlHelper.IsMySqlException(e, MySqlExceptionType.Duplicate))
                return UserAlreadyExistsError;

            throw;
        }

        return Ok(_mapper.Map<UserReadDto>(user));
    }
}