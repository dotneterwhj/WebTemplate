using Abner.Application.Core;
using FluentValidation;

namespace Abner.Application.BlogApp;

public class BlogPageQueryValidator : CommandValidator<BlogPageQuery>
{
    public BlogPageQueryValidator()
    {
        RuleFor(b => b.PageIndex).GreaterThanOrEqualTo(1);
        RuleFor(b => b.PageSize).GreaterThanOrEqualTo(1).LessThanOrEqualTo(100);
    }
}