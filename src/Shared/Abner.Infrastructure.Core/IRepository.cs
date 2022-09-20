using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abner.Domain.Core;

namespace Abner.Infrastructure.Core
{
    public interface IRepository
    { }

    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity>
        where TEntity : class, IEntity
    {
        IUnitOfWork UnitOfWork { get; }

        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task InsertManyAsync(List<TEntity> entities, CancellationToken cancellationToken = default);

        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task UpdateManyAsync(List<TEntity> entities, CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(Entity entity);
        Task DeleteManyAsync(List<TEntity> entities, CancellationToken cancellationToken = default);
    }

    public interface IRepository<TKey, TEntity> : IRepository<TEntity>, IReadOnlyRepository<TKey, TEntity>
        where TEntity : class, IEntity<TKey>
    {
        Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default);
    }
}