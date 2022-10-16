using Abner.Application.Core;
using Abner.EntityFrameworkCore.Contexts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Abner.Application.Behaviors;

public class BlogContextTransactionBehavior<TRequest, TResponse> : TransactionBehaviour<BlogContext, TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public BlogContextTransactionBehavior(BlogContext dbContext, ILogger<TransactionBehaviour<BlogContext, TRequest, TResponse>> logger) : base(dbContext, logger)
    {
    }
}