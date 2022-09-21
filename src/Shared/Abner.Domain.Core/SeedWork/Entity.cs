using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abner.Domain.Core
{
    /// <summary>
    /// 实体
    /// </summary>
    public abstract class Entity : IEntity
    {
        /// <summary>
        /// 联合主键
        /// </summary>
        /// <returns></returns>
        public abstract object[] GetKeys();

        public override string ToString()
        {
            return $"[ENTITY: {GetType().Name}] Keys = {string.Join(',', GetKeys())}";
        }

        private List<DomainEvent> _domainEvents;

        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(DomainEvent domainEventItem)
        {
            _domainEvents = _domainEvents ?? new List<DomainEvent>();
            _domainEvents.Add(domainEventItem);
        }

        public void RemoveDomainEvent(DomainEvent domainEventItem)
        {
            _domainEvents?.Remove(domainEventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }

    /// <summary>
    /// 实体
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class Entity<TKey> : Entity, IEntity<TKey>
    {
        int? _requestedHashCode;

        /// <summary>
        /// 单主键
        /// </summary>
        public virtual TKey Id { get; protected set; }

        protected Entity()
        {

        }

        protected Entity(TKey Id)
        {
            this.Id = Id;
        }

        public override object[] GetKeys()
        {
            return new object[] { Id };
        }

        public override string ToString()
        {
            return $"[ENTITY: {GetType().Name}] Id = {Id}";
        }


        public bool IsTransient()
        {
            return EqualityComparer<TKey>.Default.Equals(Id, default);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity<TKey>))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            Entity<TKey> item = (Entity<TKey>)obj;

            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return item.Id.Equals(this.Id);
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = this.Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();

        }

        public static bool operator ==(Entity<TKey> left, Entity<TKey> right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity<TKey> left, Entity<TKey> right)
        {
            return !(left == right);
        }

    }
}