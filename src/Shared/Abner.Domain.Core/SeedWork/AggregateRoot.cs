using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abner.Domain.Core
{
    /// <summary>
    /// 聚合根也是实体
    /// </summary>
    public abstract class AggregateRoot : Entity, IAggregateRoot, ISoftDelete
    {
        public bool IsDeleted { get; private set; }

        public virtual void SoftDelete()
        {
            this.IsDeleted = true;
        }
    }

    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>, ISoftDelete
    {
        public bool IsDeleted { get; private set; }

        public virtual void SoftDelete()
        {
            this.IsDeleted = true;
        }
    }
}