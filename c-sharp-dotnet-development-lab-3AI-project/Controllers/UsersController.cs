using AutoMapper;
using c_sharp_dotnet_development_lab_3AI_project.database.entities;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.dto.user;
using c_sharp_dotnet_development_lab_3AI_project.Services;
using c_sharp_dotnet_development_lab_3AI_project.Services.MySql;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace c_sharp_dotnet_development_lab_3AI_project.Controllers;

[ApiController]
[Route("/users")]
public class UsersController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRepository _repository;

    public UsersController(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<UserReadDto>> GetAllUsers() =>
        Ok(_mapper.Map<IEnumerable<UserReadDto>>(_repository.GetAllUsers()));


    [HttpGet("{id:guid}", Name = nameof(GetUser))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<UserReadDto> GetUser(Guid id)
    {
        User? user = _repository.GetUser(id);
        return user == null ? NotFound() : Ok(_mapper.Map<UserReadDto>(user));
    }

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
            if (CustomMySqlHelper.IsMySqlException(e, MySqlExceptionType.Duplicate)) return Conflict();

            throw;
        }

        return CreatedAtRoute(nameof(GetUser), new { user.Id }, _mapper.Map<UserReadDto>(user));
    }
}