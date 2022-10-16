using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abner.Domain.Core
{
    public interface IHasModificator : IHasModificationTime
    {
        string? ModificatorId { get; }
    }
}