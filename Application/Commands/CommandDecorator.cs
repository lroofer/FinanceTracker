using System.Threading.Tasks;

namespace FinanceTracker.Application.Commands
{
    public abstract class CommandDecorator<T> : ICommand<T>
    {
        protected readonly ICommand<T> _command;

        protected CommandDecorator(ICommand<T> command)
        {
            _command = command;
        }

        public abstract Task<T> ExecuteAsync();
    }
}