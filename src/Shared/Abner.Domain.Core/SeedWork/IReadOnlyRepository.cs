using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;

namespace Abner.Domain.Core
{
    /// <summary>
    /// 只读仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IReadOnlyRepository<TEntity> : IRepository
        where TEntity : class, IEntity
    {
        /// <summary>
        /// 获取多条数据
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取多条数据
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        /// <param name="includeDetails">是否包含导航属性</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> GetCountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<long> GetLongCountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="skipCount">跳过的条数</param>
        /// <param name="maxResultCount">最大取几条</param>
        /// <param name="sorting">一个表达式字符串，用于指示排序依据的值</param>
        /// <param name="includeDetails">是否包含导航属性</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetPageListAsync(int skipCount,
                                             int maxResultCount,
                                             string sorting,
                                             bool includeDetails = false,
                                             CancellationToken cancellationToken = default);

        /// <summary>
        /// 是否包含导航属性
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IQueryable<TEntity>> IncludeDetailsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 是否包含导航属性
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="propertySelectors"></param>
        /// <returns></returns>
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