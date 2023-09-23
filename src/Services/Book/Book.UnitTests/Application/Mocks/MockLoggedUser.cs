using Book.Application.Interfaces;

using Moq;

namespace Book.UnitTests.Application.Mocks;

public class MockLoggedUser : Mock<ILoggedUser>
{
    public MockLoggedUser() : base(MockBehavior.Strict) { }

    public MockLoggedUser MockId(Guid id)
    {
        Setup(s => s.Id)
            .Returns(id);

        return this;
    }
}
