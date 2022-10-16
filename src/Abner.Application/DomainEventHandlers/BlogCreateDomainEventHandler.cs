using Abner.Application.Core;
using Abner.Domain.BlogAggregate.Events;
using Microsoft.Extensions.Logging;

namespace Abner.Application.DomainEventHandlers;

public class BlogCreateDomainEventHandler : DomainEventHandler<BlogCreateDomainEvent>
{
    private readonly ILogger<BlogCreateDomainEventHandler> _logger;

    public BlogCreateDomainEventHandler(ILogger<BlogCreateDomainEventHandler> logger)
    {
        _logger = logger;
    }

    protected override void Handle(BlogCreateDomainEvent notification)
    {
        _logger.LogInformation(notification.Blog.Id.ToString());
    }
}