using Abner.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abner.Domain.BlogAggregate.Events
{
    public class BlogCreateDomainEvent : DomainEvent
    {
        public BlogCreateDomainEvent(Blog blog)
        {
            Blog = blog;
        }

        public Blog Blog { get; }
    }
}
