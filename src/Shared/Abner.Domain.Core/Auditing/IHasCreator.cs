using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abner.Domain.Core
{
    public interface IHasCreator : IHasCreator<int>, IHasCreationTime
    {
    }

    public interface IHasCreator<T> : IHasCreationTime
    {
        T CreatorId { get; }
    }
}