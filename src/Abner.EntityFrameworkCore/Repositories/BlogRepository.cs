using Abner.Domain.BlogAggregate;
using Abner.EntityFrameworkCore.Contexts;
using Abner.Infrastructure.Core;

namespace Abner.EntityFrameworkCore.Repositories
{
    public class BlogRepository : RepositoryBase<BlogContext, Guid, Blog>, IBlogRepository
    {
        public BlogRepository(BlogContext dbContext) : base(dbContext)
        {
        }
    }
}