using Abner.Application.Core;

namespace Abner.Application.Blog;

public class BlogCreateCommand : Command<BlogDto>
{
    public string Description { get; set; }

    public string Title { get; set; }

    public override bool IsValid()
    {
        throw new NotImplementedException();
    }
}