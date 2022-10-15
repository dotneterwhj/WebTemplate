using Abner.Domain.Core;

namespace Abner.Domain.BlogAggregate
{
    public interface IBlogRepository : IRepository<Guid, Blog>
    {
    }
}