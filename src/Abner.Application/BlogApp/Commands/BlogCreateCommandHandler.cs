using Abner.Application.Core;
using Abner.Domain.BlogAggregate;

namespace Abner.Application.Blog;

public class BlogCreateCommandHandler : CommandHandler<BlogCreateCommand, BlogDto>
{
    private readonly IBlogRepository _blogRepository;

    public BlogCreateCommandHandler(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    protected override async Task<BlogDto> Handle(BlogCreateCommand request)
    {
        var blog = Domain.BlogAggregate.Blog.Create(request.Title, request.Description);
        var entity = await _blogRepository.InsertAsync(blog);

        var blogDto = new BlogDto() { Title = entity.Title, Description = entity.Description, Id = entity.Id };
        return blogDto;
    }
}