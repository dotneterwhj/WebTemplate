using Abner.Application.Core;
using Abner.Domain.BlogAggregate;

namespace Abner.Application.BlogApp;

public class BlogPageQueryHandler : IQueryHandler<BlogPageQuery, List<BlogDto>>
{
    private readonly IBlogRepository _blogRepository;

    public BlogPageQueryHandler(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public async Task<List<BlogDto>> Handle(BlogPageQuery request, CancellationToken cancellationToken)
    {
        var blogs = await _blogRepository.GetPageListAsync((request.PageIndex - 1) * request.PageSize, request.PageSize,
            nameof(Blog.Id));

        // TODO Automapper
        return blogs.Select(b => new BlogDto() { Id = b.Id, Description = b.Description, Title = b.Title }).ToList();
    }
}