using System.Security.Claims;
using c_sharp_dotnet_development_lab_3AI_project.database.entities.user;
using Microsoft.IdentityModel.JsonWebTokens;

namespace c_sharp_dotnet_development_lab_3AI_project.utils;

public static class Auth
{
    public static class Jwt
    {
        /// <summary>
        ///     Retrieves and parses the <see cref="User.Id">Users Id</see> from the <see cref="ClaimsPrincipal" />.
        /// </summary>
        /// <returns>
        ///     <see cref="User.Id">User.Id</see>
        /// </returns>
        /// <example>
        ///     <code>
        /// Guid userId = Auth.Jwt.GetUserGuid(User);
        /// </code>
        /// </example>
        internal static Guid GetUserId(ClaimsPrincipal user)
        {
            return Guid.Parse(user.Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value);
        }
    }
}