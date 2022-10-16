﻿using Abner.Application.Core;

namespace Abner.Api.Host.SeedWork;

public class InvalidCommandProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
{
    public InvalidCommandProblemDetails(InvalidCommandException exception)
    {
        this.Title = exception.Message;
        this.Status = StatusCodes.Status400BadRequest;
        this.Detail = exception.Details;
        this.Type = "https://somedomain/validation-error";
        // this.Extensions?.Clear();
    }
}