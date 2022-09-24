using Abner.Domain.Core;
using MediatR;

namespace Abner.Application.Core
{
    public abstract class DomainEventHandlerBase<TDomainEvent> : NotificationHandler<TDomainEvent>, IDomainEventHandler<TDomainEvent>
        where TDomainEvent : DomainEvent
    {

    }
}
