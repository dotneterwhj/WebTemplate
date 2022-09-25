using Abner.Domain.Core;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Abner.Application.Core;

public class ValidatorBehavior<TCommand, TResult> : IPipelineBehavior<TCommand, TResult>
    where TCommand : Command<TResult>
{
    private readonly ILogger<ValidatorBehavior<TCommand, TResult>> _logger;
    private readonly IEnumerable<IValidator<TCommand>> _validators;

    public ValidatorBehavior(
        IEnumerable<IValidator<TCommand>> validators,
        ILogger<ValidatorBehavior<TCommand, TResult>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResult> Handle(TCommand request, CancellationToken cancellationToken, RequestHandlerDelegate<TResult> next)
    {
        var typeName = request.GetGenericTypeName();

        _logger.LogInformation("----- Validating command {CommandType}", typeName);

        var failures = _validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (failures.Any())
        {
            _logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName, request, failures);

            throw new DomainException(
                $"Command Validation Errors for type {typeof(TCommand).Name}", new ValidationException("Validation exception", failures));
        }

        return await next();
    }
}
