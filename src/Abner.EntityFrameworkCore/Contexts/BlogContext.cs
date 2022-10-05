using Abner.Domain.BlogAggregate;
using Abner.Infrastructure.Core;
using Microsoft.EntityFrameworkCore;

namespace Abner.EntityFrameworkCore.Contexts
{
    public class BlogContext : EFCoreContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public BlogContext(DbContextOptions options) : base(options)
        {
        }
    }
}
