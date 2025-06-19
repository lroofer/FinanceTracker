using System.Threading.Tasks;

namespace FinanceTracker.Application.Commands
{
    public interface ICommand<T>
    {
        Task<T> ExecuteAsync();
    }
}