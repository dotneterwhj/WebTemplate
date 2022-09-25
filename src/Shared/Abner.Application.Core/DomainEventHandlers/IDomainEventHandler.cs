using Abner.Domain.Core;
using MediatR;

namespace Abner.Application.Core;

public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : DomainEvent
{
}
