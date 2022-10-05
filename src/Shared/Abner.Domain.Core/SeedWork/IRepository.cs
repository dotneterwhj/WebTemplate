using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abner.Domain.Core
{
    /// <summary>
    /// 仓储标记接口
    /// </summary>
    public interface IRepository
    { }

    /// <summary>
    /// 基础仓储 包含增删改查
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> : IRepository, IReadOnlyRepository<TEntity>
        where TEntity : class, IEntity
    {
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task InsertManyAsync(List<TEntity> entities, CancellationToken cancellationToken = default);


        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateManyAsync(List<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(TEntity entity);
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteManyAsync(List<TEntity> entities, CancellationToken cancellationToken = default);
    }

    public interface IRepository<TKey, TEntity> : IRepository, IRepository<TEntity>, IReadOnlyRepository<TKey, TEntity>
        where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// 根据主键删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default);
    }
}