using Abner.Application.Core;

namespace Abner.Application.BlogApp;

public class BlogPageQuery : PageQuery<List<BlogDto>>
{
    public BlogPageQuery(int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
    }
}