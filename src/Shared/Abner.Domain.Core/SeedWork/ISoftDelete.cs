using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abner.Domain.Core
{
    public interface ISoftDelete
    {
        bool IsSoftDeleted { get; }

        void SoftDelete();
    }
}