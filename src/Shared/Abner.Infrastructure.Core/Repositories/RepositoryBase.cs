using Abner.Domain.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Abner.Infrastructure.Core;

public abstract class RepositoryBase<TDbContext, TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity
    where TDbContext : EFCoreContext
{
    protected virtual TDbContext DbContext { get; set; }

    public virtual IUnitOfWork UnitOfWork => DbContext;

    protected virtual IGuidGenerator GuidGenerator { get; set; } = new SimpleGuidGenerator();

    public RepositoryBase(TDbContext dbContext)
    {
        DbContext = dbContext;
    }

    protected virtual TDbContext GetDbContext()
    {
        return DbContext;
    }

    protected virtual DbSet<TEntity> GetDbSet()
    {
        return GetDbContext().Set<TEntity>();
    }

    protected virtual IQueryable<TEntity> GetQueryable()
    {
        return GetDbSet().AsQueryable();
    }

    public virtual Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return GetDbSet().CountAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return includeDetails ?
            await (await IncludeDetailsAsync(cancellationToken)).ToListAsync(cancellationToken) :
            await GetDbSet().ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var result = GetDbSet().Where(predicate);
        if (includeDetails)
        {
            result = await IncludeDetailsAsync(cancellationToken);
        }
        return await result.ToListAsync(cancellationToken);
    }

    public virtual Task<long> GetLongCountAsync(CancellationToken cancellationToken = default)
    {
        return GetDbSet().LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetPageListAsync(int skipCount, int maxResultCount, string sorting, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var queryable = includeDetails ?
            await IncludeDetailsAsync(cancellationToken) :
            GetQueryable();

        return await queryable
                    .OrderByIf(!string.IsNullOrEmpty(sorting), sorting)
                    .PageBy(skipCount, maxResultCount)
                    .ToListAsync();
    }

    public virtual Task<IQueryable<TEntity>> IncludeDetailsAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual Task<IQueryable<TEntity>> IncludeDetailsAsync(CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] propertySelectors)
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

    public virtual async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
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

        TrySetId(entity, () => GuidGenerator.Create());
    }

    public static void TrySetId<TKey>(
       IEntity<TKey> entity,
       Func<TKey> idFactory)
    {
        var idProperty = entity.GetType().GetProperties().FirstOrDefault(p => p.Name == nameof(entity.Id));
        idProperty?.SetValue(entity, idFactory.Invoke());
    }

    public virtual async Task InsertManyAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await InsertAsync(entity, cancellationToken);
        }
    }

    public virtual Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var updatedEntity = GetDbSet().Update(entity).Entity;

        return Task.FromResult(updatedEntity);
    }

    public virtual async Task UpdateManyAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await UpdateAsync(entity, cancellationToken);
        }
    }

    public virtual async Task<bool> DeleteAsync(TEntity entity)
    {
        var existEntity = await GetDbSet().FindAsync(entity.GetKeys());
        if (existEntity != null)
        {
            GetDbSet().Remove(existEntity);
        }
        return true;
    }

    public virtual async Task DeleteManyAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await DeleteAsync(entity);
        }
    }

}


public abstract class RepositoryBase<TDbContext, TKey, TEntity> : RepositoryBase<TDbContext, TEntity>, IRepository<TKey, TEntity>
    where TEntity : class, IEntity<TKey>
    where TDbContext : EFCoreContext
{
    protected RepositoryBase(TDbContext dbContext) : base(dbContext)
    {
    }

    public virtual async Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await base.GetDbSet().FindAsync(id, cancellationToken);

        if (entity != null)
            base.GetDbSet().Remove(entity);

        return true;
    }

    public virtual async Task<TEntity> FindAsync(TKey id, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return includeDetails ?
            await (await base.IncludeDetailsAsync(cancellationToken)).FirstOrDefaultAsync(t => t.Id.Equals(id), cancellationToken) :
            await base.GetDbSet().FindAsync(new object[] { id }, cancellationToken);
    }

    public virtual async Task<TEntity> GetAsync(TKey id, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(id, includeDetails, cancellationToken);
        if (entity == null)
        {
            throw new KeyNotFoundException(nameof(id));
        }
        return entity;
    }
}

