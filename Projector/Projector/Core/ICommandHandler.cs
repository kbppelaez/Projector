namespace Projector.Core
{
    public interface ICommandHandler<TCommand> where TCommand : class
    {
        Task<CommandResult> Execute(TCommand args);
    }
}
