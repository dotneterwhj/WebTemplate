using Abner.Application.Core;

namespace Abner.Application.BlogApp;

public class BlogCreateCommand : Command<BlogDto>
{
    public string Description { get; set; }

    public string Title { get; set; }

    public BlogCreateCommand(string title, string description)
    {
        Title = title;
        Description = description;
    }
}