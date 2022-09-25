using Abner.Infrastructure.Core;
using Microsoft.EntityFrameworkCore;

namespace Abner.EntityFrameworkCore.Contexts
{
    public class BlogContext : EFCoreContext
    {
        public BlogContext(DbContextOptions options) : base(options)
        {
        }
    }
}
