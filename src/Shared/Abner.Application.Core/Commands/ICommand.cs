using MediatR;

namespace Abner.Application.Core;

public interface ICommand : IRequest
{
    Guid CommandId { get; }
}

public interface ICommand<out TResult> : IRequest<TResult>
{
    Guid CommandId { get; }
}
