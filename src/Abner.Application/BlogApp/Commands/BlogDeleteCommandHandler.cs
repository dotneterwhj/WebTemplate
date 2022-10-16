using Abner.Application.Core;
using Abner.Domain.BlogAggregate;

namespace Abner.Application.BlogApp;

public class BlogDeleteCommandHandler : ICommandHandler<BlogDeleteCommand, bool>
{
    private readonly IBlogRepository _blogRepository;

    public BlogDeleteCommandHandler(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public Task<bool> Handle(BlogDeleteCommand request, CancellationToken cancellationToken)
    {
        return _blogRepository.DeleteAsync(request.Id);
    }
}