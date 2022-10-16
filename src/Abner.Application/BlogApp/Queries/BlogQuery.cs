using Abner.Application.Core;

namespace Abner.Application.Blog;

public class BlogQuery : QueryBase<BlogDto>
{
    public Guid Id { get; set; }
}