using Book.API.Extensions;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Book.UnitTests.API.Extensions;

public class ClaimsPrincipalExtensionsTests
{
    [Fact]
    public void GetUserId_WithoutClaim_ThrowInvalidOperation()
    {
        // Arrange
        var principal = new ClaimsPrincipal();

        // Act
        Action action = () => principal.GetUserId();

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Could not found user identifier");
    }

    [Fact]
    public void GetUserId_WithSubAndNameIdentifier_ReturnSubClaimValue()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var principal = new ClaimsPrincipal();
        var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };
        principal.AddIdentity(new ClaimsIdentity(claims));

        // Act
        string result = principal.GetUserId();

        // Assert
        result.Should().Be(userId.ToString());
    }

    [Fact]
    public void GetUserId_WithoutSubClaim_ReturnNameIdentifierClaimValue()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var principal = new ClaimsPrincipal();
        var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };
        principal.AddIdentity(new ClaimsIdentity(claims));

        // Act
        string result = principal.GetUserId();

        // Assert
        result.Should().Be(userId.ToString());
    }
}
