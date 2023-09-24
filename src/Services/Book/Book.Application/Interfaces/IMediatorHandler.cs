using Book.Domain.Common;

namespace Book.Application.Interfaces;

public interface IMediatorHandler
{
    Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : BaseCommand;

    Task<TResponse> Send<TResponse>(BaseQuery<TResponse> request, CancellationToken cancellationToken = default);
}
