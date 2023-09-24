using Book.API.Identity;
using Book.UnitTests.API.Mocks;

using Microsoft.AspNetCore.Http;

using System.Security.Claims;

namespace Book.UnitTests.API.Identity;

public class LoggedUserTests
{
    private readonly MockHttpContextAccessor _contextAccessor;
    private readonly LoggedUser _loggedUser;

    public LoggedUserTests()
    {
        _contextAccessor = new();
        _loggedUser = new(_contextAccessor.Object);
    }

    [Fact]
    public void GetUserId_HttpContextIsNull_ThrowInvalidOperation()
    {
        // Arrange
        _contextAccessor
            .MockHttpContext(httpContext: null);

        // Act
        Action action = () => _loggedUser.GetUserId();

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("HttpContext could not be found");
    }

    [Fact]
    public void GetUserId_HasHttpContext_GetUserIdFromClaims()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var principal = new ClaimsPrincipal();
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
        };
        principal.AddIdentity(new ClaimsIdentity(claims));

        var httpContext = new DefaultHttpContext()
        {
            User = principal,
        };

        _contextAccessor
            .MockHttpContext(httpContext);

        // Act
        Guid result = _loggedUser.GetUserId();

        // Assert
        result.Should().Be(userId);
    }
}
