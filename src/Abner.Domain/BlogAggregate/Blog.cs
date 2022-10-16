﻿using Abner.Domain.BlogAggregate.Events;
using Abner.Domain.Core;

namespace Abner.Domain.BlogAggregate;

public class Blog : FullAuditAggregateRoot<Guid>
{
    public string Title { get; private set; }
    public string Description { get; private set; }

    protected Blog()
    {
    }

    public Blog(string title, string description)
    {
        Title = title;
        Description = description;
        Id = Guid.NewGuid();
        AddDomainEvent(new BlogCreateDomainEvent(this));
    }

    public static Blog Create(string title, string description)
    {
        var blog = new Blog(title, description);
        return blog;
    }
}