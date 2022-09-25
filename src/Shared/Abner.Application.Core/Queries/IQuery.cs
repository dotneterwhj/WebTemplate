using MediatR;

namespace Abner.Application.Core;

public interface IQuery<out TResult> : IRequest<TResult>
{
}
