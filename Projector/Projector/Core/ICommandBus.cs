using System.Reflection;

namespace Projector.Core
{
    public interface ICommandBus
    {
        Task<CommandResult> ExecuteAsync<TCmd>(TCmd command);
    }

    public class CommandBus : ICommandBus
    {
        private readonly IServiceProvider _serviceprovider;

        public CommandBus(IServiceProvider serviceProvider)
        {
            _serviceprovider = serviceProvider;
        }

        public Task<CommandResult> ExecuteAsync<Tcmd>(Tcmd args)
        {
            Type handlerContract = typeof(ICommandHandler<>);
            Type handlerType = handlerContract.MakeGenericType(typeof(Tcmd));

            object handlerInstance = _serviceprovider.GetService(handlerType);

            MethodInfo executeMethod = handlerType.GetMethod("Execute", BindingFlags.Public | BindingFlags.Instance);

            var resultTask = (Task<CommandResult>) executeMethod.Invoke(handlerInstance, new object[] { args });

            return resultTask;
        }
    }
}
