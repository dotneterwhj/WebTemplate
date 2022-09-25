namespace Abner.Application.Core;

public abstract class Command : ICommand
{
    private Guid _commandId;
    public Guid CommandId => _commandId;
    public Command()
    {
        _commandId = Guid.NewGuid();
    }

    protected Command(Guid id)
    {
        this._commandId = id;
    }

    public abstract bool IsValid();
}

public abstract class Command<TResult> : ICommand<TResult>
{
    private Guid _commandId;
    public Guid CommandId => _commandId;
    public Command()
    {
        _commandId = Guid.NewGuid();
    }

    protected Command(Guid id)
    {
        this._commandId = id;
    }

    public abstract bool IsValid();
}
