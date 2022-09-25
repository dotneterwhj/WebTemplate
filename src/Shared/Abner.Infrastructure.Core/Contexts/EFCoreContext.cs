using Microsoft.EntityFrameworkCore;
using DotNetCore.CAP;
using MediatR;
using Abner.Domain.Core;
using Microsoft.EntityFrameworkCore.Storage;

namespace Abner.Infrastructure.Core;

public abstract class EFCoreContext : DbContext, IUnitOfWork
{

    private IDbContextTransaction? _currentTransaction;

    // 属性注入
    protected IMediator Mediator { get;  set; }
    protected ICapPublisher CapPublisher { get;  set; }

    public EFCoreContext(DbContextOptions options)
        //IMediator mediator,
        //ICapPublisher capPublisher)
        : base(options)
    {
        //this.Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator)); ;
        //this.CapPublisher = capPublisher ?? throw new ArgumentNullException(nameof(capPublisher)); ;
    }

    #region Transaction

    public IDbContextTransaction? GetCurrentTransaction() => _currentTransaction;

    public bool HasActiveTransaction => _currentTransaction != null;

    public Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (HasActiveTransaction) return Task.FromResult(_currentTransaction!);

        // this.Database.BeginTransaction()
        this._currentTransaction = this.Database.BeginTransaction(CapPublisher, autoCommit: false);

        return Task.FromResult(_currentTransaction);

    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await base.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 启用全局软删除查询
        modelBuilder.EnableGloableSoftDeleteQueryFilter();
        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        // Dispatch Domain Events collection. 
        // Choices:
        // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
        // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
        // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
        // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
        await Mediator.DispatchDomainEventsAsync(this, cancellationToken);

        // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
        // performed through the DbContext will be committed
        var result = await SaveChangesAsync(cancellationToken);

        return true;
    }

    public override int SaveChanges()
    {
        OnBeforeSaving();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        OnBeforeSaving();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void OnBeforeSaving()
    {
        // 标记软删除但未标记硬删除
        var softDeletes = ChangeTracker.Entries<Entity>()
            .Where(e => e.Entity.GetType().IsAssignableTo(typeof(ISoftDelete)) && !e.Entity.GetType().IsAssignableTo(typeof(IHardDelete)));

        foreach (var entry in softDeletes)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.CurrentValues[nameof(ISoftDelete.IsDeleted)] = false;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.CurrentValues[nameof(ISoftDelete.IsDeleted)] = true;
                    break;
            }
        }
    }
}