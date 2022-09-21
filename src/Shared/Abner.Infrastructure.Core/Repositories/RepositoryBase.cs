using Abner.Domain.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Abner.Infrastructure.Core
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        protected virtual EFCoreContext DbContext { get; set; }

        public virtual IUnitOfWork UnitOfWork => DbContext;

        public RepositoryBase(EFCoreContext dbContext)
        {
            DbContext = dbContext;
        }

        protected EFCoreContext GetDbContext()
        {
            return DbContext;
        }

        protected DbSet<TEntity> GetDbSet()
        {
            return DbContext.Set<TEntity>();
        }

        protected IQueryable<TEntity> GetQueryable()
        {
            return GetDbSet().AsQueryable();
        }

        public Task<int> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return GetDbSet().CountAsync(cancellationToken);
        }

        public async Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return includeDetails ?
                await GetDbSet().ToListAsync(cancellationToken) :
                await (await IncludeDetailsAsync(cancellationToken)).ToListAsync(cancellationToken);
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var result = GetDbSet().Where(predicate);
            if (includeDetails)
            {
                result = await IncludeDetailsAsync(cancellationToken);
            }
            return await result.ToListAsync(cancellationToken);
        }

        public Task<long> GetLongCountAsync(CancellationToken cancellationToken = default)
        {
            return GetDbSet().LongCountAsync(cancellationToken);
        }

        public async Task<List<TEntity>> GetPageListAsync(int skipCount, int maxResultCount, string sorting, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var queryable = includeDetails ?
                GetQueryable() :
                await IncludeDetailsAsync(cancellationToken);

            return await queryable.OrderByIf(!string.IsNullOrEmpty(sorting), sorting).PageBy(skipCount, maxResultCount).ToListAsync();
        }

        public Task<IQueryable<TEntity>> IncludeDetailsAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<TEntity>> IncludeDetailsAsync(CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = GetQueryable();
            if (propertySelectors != null)
            {
                IncludeDetails(query, propertySelectors);
            }
            return Task.FromResult(query);
        }

        private static IQueryable<TEntity> IncludeDetails(IQueryable<TEntity> query, Expression<Func<TEntity, object>>[] propertySelectors)
        {
            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }
            return query;
        }

        public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity is IEntity<Guid> emptyIdEntity)
            {
                TrySetGuidId(emptyIdEntity);
            }
            var savedEntity = (await GetDbSet().AddAsync(entity, cancellationToken)).Entity;
            return savedEntity;
        }

        protected virtual void TrySetGuidId(IEntity<Guid> entity)
        {
            if (entity.Id != default)
            {
                return;
            }
            // TODO 利用反射赋值
            //entity.Id = Guid.NewGuid();
        }

        public async Task InsertManyAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                await InsertAsync(entity, cancellationToken);
            }
        }

        public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var updatedEntity = GetDbSet().Update(entity).Entity;

            return Task.FromResult(updatedEntity);
        }

        public async Task UpdateManyAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                await UpdateAsync(entity, cancellationToken);
            }
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            var existEntity = await GetDbSet().FindAsync(entity.GetKeys());
            if (existEntity != null)
            {
                GetDbSet().Remove(existEntity);
            }
            return true;
        }

        public async Task DeleteManyAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                await DeleteAsync(entity);
            }
        }

    }
}
