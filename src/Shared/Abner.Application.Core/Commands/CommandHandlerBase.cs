using MediatR;

namespace Abner.Application.Core.Commands
{
    public abstract class CommandHandlerBase<TCommand, TResult> : RequestHandler<TCommand, TResult>, IRequestHandler<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
    }
}
