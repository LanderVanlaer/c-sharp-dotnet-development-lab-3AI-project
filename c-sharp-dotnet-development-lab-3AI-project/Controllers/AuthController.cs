using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.auth.dto;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.user;
using c_sharp_dotnet_development_lab_3AI_project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace c_sharp_dotnet_development_lab_3AI_project.Controllers;

/// <summary>
///     https://www.c-sharpcorner.com/article/jwt-json-web-token-authentication-in-asp-net-core/
/// </summary>
[ApiController]
[Route("/auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IRepository _repository;

    public AuthController(IRepository repository, IConfiguration config)
    {
        _repository = repository;
        _config = config;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult Login(UserLoginDto dto)
    {
        User? user = AuthenticateUser(dto);

        if (user == null) return Unauthorized();

        string tokenString = GenerateJwtToken(user);

        return Ok(new { token = tokenString });
    }

    private string GenerateJwtToken(User user)
    {
        SymmetricSecurityKey securityKey =
            new(Encoding.UTF8.GetBytes(_config["JwtSettings:SigningKey"] ??
                                       throw new InvalidOperationException("Config JwtSettings:SigningKey is null")));
        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        Claim[] claims =
        {
            new(JwtRegisteredClaimNames.Sub,
                user.Username), //https://datatracker.ietf.org/doc/html/rfc7519#section-4.1.2
            new(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
        };

        JwtSecurityToken token = new(
            _config["JwtSettings:Issuer"],
            _config["JwtSettings:Issuer"],
            claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private User? AuthenticateUser(UserLoginDto dto)
    {
        User? user = _repository.GetUserByUserName(dto.Username);

        if (user == null) return null;

        return new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, dto.Password) ==
               PasswordVerificationResult.Failed
            ? null
            : user;
    }
}