using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abner.Domain.Core
{
    /// <summary>
    /// 聚合根标记接口
    /// </summary>
    public interface IAggregateRoot : IEntity
    { }


    public interface IAggregateRoot<TKey> : IEntity<TKey>, IAggregateRoot
    { }
}