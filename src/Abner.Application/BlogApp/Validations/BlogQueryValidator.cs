using Abner.Application.Core;
using FluentValidation;

namespace Abner.Application.BlogApp;

public class BlogQueryValidator : CommandValidator<BlogQuery>
{
    public BlogQueryValidator()
    {
        RuleFor(b => b.Id).NotEmpty();
        // RuleFor(b => b.Id.ToString()).Length(20);
    }
}