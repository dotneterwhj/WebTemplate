using Abner.Application.Core;

namespace Abner.Application.BlogApp;

public class BlogQuery : Query<BlogDto>
{
    public Guid Id { get; set; }
}