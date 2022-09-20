using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abner.Domain.Core
{
    /// <summary>
    /// Defines an entity. It's primary key may not be "Id" or it may have a composite primary key.
    /// Use <see cref="IEntity{TKey}"/> where possible for better integration to repositories and other structures in the framework.
    /// </summary>
    public interface IEntity
    {
        object[] GetKeys();
    }

    /// <summary>
    /// Defines an entity with a single primary key with "Id" property.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; }
    }
}