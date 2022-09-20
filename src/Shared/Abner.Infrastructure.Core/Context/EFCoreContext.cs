using Microsoft.EntityFrameworkCore;
using DotNetCore.CAP;
using MediatR;
using Abner.Domain.Core;
using Microsoft.EntityFrameworkCore.Storage;

namespace Abner.Infrastructure.Core
{
    public abstract class EFCoreContext : DbContext, IUnitOfWork
    {

        private IDbContextTransaction? _currentTransaction;

        protected readonly IMediator _mediator;
        protected readonly ICapPublisher _capPublisher;

        public EFCoreContext(DbContextOptions<EFCoreContext> options,
                             IMediator mediator,
                             ICapPublisher capPublisher)
            : base(options)
        {
            this._mediator = mediator ?? throw new ArgumentNullException(nameof(mediator)); ;
            this._capPublisher = capPublisher ?? throw new ArgumentNullException(nameof(capPublisher)); ;
        }

        public IDbContextTransaction? GetCurrentTransaction() => _currentTransaction;

        public bool HasActiveTransaction => _currentTransaction != null;

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
            await _mediator.DispatchDomainEventsAsync(this);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            var result = await base.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (HasActiveTransaction) return Task.FromResult(_currentTransaction!);

            // this.Database.BeginTransaction()
            this._currentTransaction = this.Database.BeginTransaction(_capPublisher);

            return Task.FromResult(_currentTransaction);

        }

    }
}