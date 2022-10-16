using Abner.Domain.Core;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Abner.Application.Core;

public class ValidatorBehavior<TCommand, TResult> : IPipelineBehavior<TCommand, TResult>
    where TCommand : IRequest<TResult>
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

    public async Task<TResult> Handle(TCommand request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResult> next)
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
            _logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}",
                typeName, request, failures);

            throw new InvalidCommandException(
                $"Command Validation Errors for type {typeof(TCommand).Name}",
                string.Join(Environment.NewLine, failures?.Select(f => f.ToString())));
        }

        var response = await next();

        _logger.LogInformation("----- command {CommandType} Validated", typeName);

        return response;
    }
}