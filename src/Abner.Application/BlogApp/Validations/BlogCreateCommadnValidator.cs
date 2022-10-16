using Abner.Application.Core;
using FluentValidation;

namespace Abner.Application.BlogApp;

public class BlogCreateCommadnValidator : CommandValidator<BlogCreateCommand>
{
    public BlogCreateCommadnValidator()
    {
        RuleFor(b => b.Description).NotEmpty();
        RuleFor(b => b.Title).NotEmpty();
    }
}