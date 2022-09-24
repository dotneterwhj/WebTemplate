namespace Abner.Application.Core
{
    public abstract class CommandBase : ICommand
    {
        private Guid _commandId;
        public Guid CommandId => _commandId;
        public CommandBase()
        {
            _commandId = Guid.NewGuid();
        }

        protected CommandBase(Guid id)
        {
            this._commandId = id;
        }
    }

    public abstract class CommandBase<TResult> : ICommand<TResult>
    {
        private Guid _commandId;
        public Guid CommandId => _commandId;
        public CommandBase()
        {
            _commandId = Guid.NewGuid();
        }

        protected CommandBase(Guid id)
        {
            this._commandId = id;
        }
    }
}
