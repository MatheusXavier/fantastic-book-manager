using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Book.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal, nameof(principal));

        Claim? claim = principal.FindFirst(JwtRegisteredClaimNames.Sub);

        claim ??= principal.FindFirst(ClaimTypes.NameIdentifier);

        return claim?.Value ?? throw new InvalidOperationException("Could not found user identifier");
    }
}