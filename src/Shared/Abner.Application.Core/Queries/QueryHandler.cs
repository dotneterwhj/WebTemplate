using MediatR;

namespace Abner.Application.Core;

public abstract class QueryHandler<TQuery, TResult> : RequestHandler<TQuery, TResult>, IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
}
