using MediatR;

namespace Abner.Domain.Core;

public interface IDomainEvent : INotification
{

}

public abstract class DomainEvent : IDomainEvent
{}