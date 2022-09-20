using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abner.Domain.Core
{
    /// <summary>
    /// 聚合根也是实体
    /// </summary>
    public abstract class AggregateRoot : Entity, IAggregateRoot, ISoftDelete, IHasCreator, IHasDeletor, IHasModificator
    {
        public DateTime CreationTime { get; private set; } = DateTime.Now;
        public int CreatorId { get; private set; }

        public DateTime? ModificationTime { get; private set; }
        public int ModificatorId { get; private set; }

        public DateTime? DeletionTime { get; private set; }
        public int DeletorId { get; private set; }

        public bool IsSoftDeleted { get; private set; }

        public void SoftDelete()
        {
            this.IsSoftDeleted = true;
            this.DeletionTime = DateTime.Now;
        }

        public void Modify()
        {
            this.ModificationTime = DateTime.Now;
        }
    }

    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>, ISoftDelete, IHasCreator<TKey>, IHasDeletor<TKey>, IHasModificator<TKey>
    {
        public DateTime CreationTime { get; private set; } = DateTime.Now;
        public TKey CreatorId { get; private set; }

        public DateTime? ModificationTime { get; private set; }
        public TKey ModificatorId { get; private set; }

        public DateTime? DeletionTime { get; private set; }
        public TKey DeletorId { get; private set; }

        public bool IsSoftDeleted { get; private set; }

        public void SoftDelete()
        {
            this.IsSoftDeleted = true;
            this.DeletionTime = DateTime.Now;
        }

        public void Modify()
        {
            this.ModificationTime = DateTime.Now;
        }
    }
}