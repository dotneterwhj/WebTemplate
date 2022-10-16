using Abner.Application.Core;

namespace Abner.Application.BlogApp;

public class BlogQuery : QueryBase<BlogDto>
{
    public Guid Id { get; set; }
}