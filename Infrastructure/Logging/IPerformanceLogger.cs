using System.Threading.Tasks;

namespace FinanceTracker.Infrastructure.Logging
{
    public interface IPerformanceLogger
    {
        Task LogPerformanceAsync(string operationName, long elapsedMilliseconds);
    }
}