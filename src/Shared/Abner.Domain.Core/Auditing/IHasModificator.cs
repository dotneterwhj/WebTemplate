using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abner.Domain.Core
{
    public interface IHasModificator : IHasModificator<int>, IHasModificationTime
    {
    }

    public interface IHasModificator<T> : IHasModificationTime
    {
        T ModificatorId { get; }
    }
}