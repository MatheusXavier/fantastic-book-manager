using Book.Application.Interfaces;
using Book.Domain.Common;

using Moq;

namespace Book.UnitTests.Infrastructure.Mocks;

public class MockMediatorHandler : Mock<IMediatorHandler>
{
    public MockMediatorHandler() : base(MockBehavior.Strict) { }

    public MockMediatorHandler MockSend<TRequest>(
        TRequest request,
        CancellationToken cancellationToken = default) where TRequest : BaseCommand
    {
        Setup(s => s.Send(request, cancellationToken))
            .Returns(Task.CompletedTask);

        return this;
    }
}