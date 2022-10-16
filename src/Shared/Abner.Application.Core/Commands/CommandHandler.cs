using MediatR;

namespace Abner.Application.Core;

public abstract class CommandHandler<TCommand> : IRequestHandler<TCommand>
    where TCommand : Command
{
    public Task<Unit> Handle(TCommand request, CancellationToken cancellationToken)
    {
        // if (request.IsValid())
        // {
            // Handle(request);
        // }
        
        Handle(request);
        
        return Unit.Task;
    }

    protected abstract void Handle(TCommand request);
}

public abstract class CommandHandler<TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : Command<TResult>
{
    public Task<TResult> Handle(TCommand request, CancellationToken cancellationToken)
    {
        // if (request.IsValid())
        // {
            // return Handle(request);
        // }
        
        var response = Handle(request);

        return Task.FromResult(response);
    }

    protected abstract TResult Handle(TCommand request);
}