using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using Abner.Domain.Core;

namespace Abner.Infrastructure.Core
{
    public interface IReadOnlyRepository<TEntity> : IRepository
        where TEntity : class, IEntity
    {
        Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default);

        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = false, CancellationToken cancellationToken = default);

        Task<int> GetCountAsync(CancellationToken cancellationToken = default);

        Task<long> GetLongCountAsync(CancellationToken cancellationToken = default);

        Task<List<TEntity>> GetPageListAsync(int skipCount,
                                             int maxResultCount,
                                             string sorting,
                                             bool includeDetails = false,
                                             CancellationToken cancellationToken = default);

        Task<IQueryable<TEntity>> IncludeDetailsAsync(CancellationToken cancellationToken = default);

        Task<IQueryable<TEntity>> IncludeDetailsAsync(CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] propertySelectors);
    }

    public interface IReadOnlyRepository<TKey, TEntity> : IReadOnlyRepository<TEntity>, IRepository<TEntity>
        where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// if not find throw an exception
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetAsync(TKey id, bool includeDetails = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// if not find return null
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> FindAsync(TKey id, bool includeDetails = false, CancellationToken cancellationToken = default);
    }
}