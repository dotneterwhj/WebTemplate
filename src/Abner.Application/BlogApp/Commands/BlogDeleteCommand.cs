using Abner.Application.Core;

namespace Abner.Application.BlogApp;

public class BlogDeleteCommand : Command<bool>
{
    public Guid Id { get; }

    public BlogDeleteCommand(Guid id)
    {
        Id = id;
    }
}