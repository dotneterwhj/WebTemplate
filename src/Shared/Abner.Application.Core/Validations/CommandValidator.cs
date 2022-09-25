using FluentValidation;

namespace Abner.Application.Core
{
    public abstract class CommandValidator<TCommand> : AbstractValidator<TCommand>
        where TCommand : Command
    {
        public CommandValidator()
        {

        }
    }
}
