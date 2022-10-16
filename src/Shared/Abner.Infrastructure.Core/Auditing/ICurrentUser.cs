namespace Abner.Infrastructure.Core;

public interface ICurrentUser
{
    string UserId { get; }

    bool IsAuthenticated { get; }
}