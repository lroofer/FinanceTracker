using System;
using System.Threading.Tasks;

namespace FinanceTracker.Application.Logging
{
    public class ConsolePerformanceLogger : IPerformanceLogger
    {
        public Task LogPerformanceAsync(string operationName, long elapsedMilliseconds)
        {
            Console.WriteLine($"[PERFORMANCE] Operation '{operationName}' took {elapsedMilliseconds}ms to complete");
            return Task.CompletedTask;
        }
    }
}