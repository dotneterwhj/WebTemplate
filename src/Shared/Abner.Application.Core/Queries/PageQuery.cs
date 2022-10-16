using System.Collections;

namespace Abner.Application.Core;

public class PageQuery<TResult> : Query<TResult>
    where TResult : IEnumerable
{
    public int PageSize { get; set; }

    public int PageIndex { get; set; }
}