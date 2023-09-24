using Book.Application.Interfaces;
using Book.Domain.Common;

using MediatR;

namespace Book.Infrastructure.Services;

public class MediatorHandler : IMediatorHandler
{
    private readonly IMediator _mediator;

    public MediatorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Send<TRequest>(
        TRequest request,
        CancellationToken cancellationToken = default) where TRequest : BaseCommand
    {
        cancellationToken.ThrowIfCancellationRequested();
        await _mediator.Send(request, cancellationToken);
    }

    public async Task<TResponse> Send<TResponse>(
        BaseQuery<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await _mediator.Send(request, cancellationToken);
    }
}
