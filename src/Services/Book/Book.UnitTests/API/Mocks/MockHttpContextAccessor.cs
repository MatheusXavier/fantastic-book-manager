using Microsoft.AspNetCore.Http;

using Moq;

namespace Book.UnitTests.API.Mocks;

public class MockHttpContextAccessor : Mock<IHttpContextAccessor>
{
    public MockHttpContextAccessor() : base(MockBehavior.Strict)
    {
    }

    public MockHttpContextAccessor MockHttpContext(HttpContext? httpContext)
    {
        Setup(s => s.HttpContext).Returns(httpContext);

        return this;
    }
}

