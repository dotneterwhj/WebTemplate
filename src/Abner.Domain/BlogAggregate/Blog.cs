using Abner.Domain.BlogAggregate.Events;
using Abner.Domain.Core;

namespace Abner.Domain.BlogAggregate;

public class Blog : AggregateRoot<Guid>
{
    public string Title { get; private set; }
    public string Description { get; private set; }

    protected Blog()
    {
        AddDomainEvent(new BlogCreateDomainEvent(this));
    }

    public static Blog Create(string title, string description)
    {
        var blog = new Blog
        {
            Title = title,
            Description = description,
            Id = Guid.NewGuid()
        };

        return blog;
    }

}
