using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abner.Domain.Core
{
    public interface IHasDeletor : IHasDeletor<int>, IHasDeletionTime
    {
    }

    public interface IHasDeletor<T> : IHasDeletionTime
    {
        T DeletorId { get; }
    }
}