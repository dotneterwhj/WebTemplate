using Abner.Application.Core;
using Abner.Domain.BlogAggregate;

namespace Abner.Application.BlogApp;

public class BlogCreateCommandHandler : ICommandHandler<BlogCreateCommand, BlogDto>
{
    private readonly IBlogRepository _blogRepository;

    public BlogCreateCommandHandler(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public async Task<BlogDto> Handle(BlogCreateCommand request, CancellationToken cancellationToken)
    {
        var blog = Domain.BlogAggregate.Blog.Create(request.Title, request.Description);
        var entity = await _blogRepository.InsertAsync(blog, cancellationToken);

        var blogDto = new BlogDto() { Title = entity.Title, Description = entity.Description, Id = entity.Id };
        return blogDto;
    }
}