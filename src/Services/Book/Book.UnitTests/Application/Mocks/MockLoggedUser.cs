using Book.Application.Interfaces;

using Moq;

namespace Book.UnitTests.Application.Mocks;

public class MockLoggedUser : Mock<ILoggedUser>
{
    public MockLoggedUser() : base(MockBehavior.Strict) { }

    public MockLoggedUser MockGetUserId(Guid id)
    {
        Setup(s => s.GetUserId())
            .Returns(id);

        return this;
    }
}
