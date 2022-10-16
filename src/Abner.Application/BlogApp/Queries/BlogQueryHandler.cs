using Abner.Application.Core;
using Abner.Domain.BlogAggregate;

namespace Abner.Application.BlogApp;

public class BlogQueryHandler : IQueryHandler<BlogQuery, BlogDto>
{
    private readonly IBlogRepository _blogRepository;

    public BlogQueryHandler(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public async Task<BlogDto> Handle(BlogQuery request, CancellationToken cancellationToken)
    {
        var blog = await _blogRepository.GetAsync(request.Id, cancellationToken: cancellationToken);

        // TODO Automapper
        var blogDto = new BlogDto()
        {
            Id = blog.Id,
            Description = blog.Description,
            Title = blog.Title
        };

        return blogDto;
    }
}