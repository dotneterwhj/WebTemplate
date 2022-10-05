using Abner.Domain.Core;
using MediatR;

namespace Abner.Application.Core;

public abstract class DomainEventHandler<TDomainEvent> : NotificationHandler<TDomainEvent>, IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{

}
