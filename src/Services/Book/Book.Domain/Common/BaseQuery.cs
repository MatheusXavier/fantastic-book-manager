using MediatR;

namespace Book.Domain.Common;

public abstract record BaseQuery<TResult> : IRequest<TResult>;