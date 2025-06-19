using System.Threading.Tasks;

namespace FinanceTracker.Application.Logging
{
    public interface IPerformanceLogger
    {
        Task LogPerformanceAsync(string operationName, long elapsedMilliseconds);
    }
}
