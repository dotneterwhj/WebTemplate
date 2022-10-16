namespace Abner.Application.Core;

public class InvalidCommandException : Exception
{
    public string? Details { get; }

    public InvalidCommandException()
    {
    }

    public InvalidCommandException(string message, string? details = null)
        : base(message)
    {
        Details = details;
    }

    public InvalidCommandException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}