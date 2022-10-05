using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abner.Domain.Core
{
    /// <summary>
    /// 软删除标记接口
    /// </summary>
    public interface ISoftDelete
    {
        bool IsDeleted { get; }

        void SoftDelete();
    }
}