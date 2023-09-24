using Book.Application.Interfaces;
using Book.Domain.Common;
using Book.Domain.Results;

using Moq;

namespace Book.UnitTests.Application.Mocks;

public class MockErrorHandler : Mock<IErrorHandler>
{
    public MockErrorHandler() : base(MockBehavior.Strict) { }

    public MockErrorHandler MockValidateCommand(BaseCommand command, bool isValid)
    {
        Setup(s => s.ValidateCommand(command))
            .Returns(isValid);

        return this;
    }

    public MockErrorHandler MockAdd(ErrorDetail errorDetail)
    {
        Setup(s => s.Add(errorDetail));

        return this;
    }

    public MockErrorHandler MockHasError(bool output)
    {
        Setup(s => s.HasError())
            .Returns(output);

        return this;
    }

    public MockErrorHandler MockGetError(ErrorResult output)
    {
        Setup(s => s.GetError())
            .Returns(output);

        return this;
    }
}
