using Abner.Domain.BlogAggregate;
using Abner.Infrastructure.Core;

namespace Abner.EntityFrameworkCore.Repositories
{
    public class BlogRepository : RepositoryBase<Blog>, IBlogRepository
    {
        public BlogRepository(EFCoreContext dbContext) : base(dbContext)
        {
        }
    }
}
