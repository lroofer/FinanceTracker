using System.Diagnostics;
using System.Threading.Tasks;
using FinanceTracker.Application.Logging;

namespace FinanceTracker.Application.Commands
{
    public class PerformanceMonitoringDecorator<T> : CommandDecorator<T>
    {
        private readonly IPerformanceLogger _logger;
        private readonly string _commandName;

        public PerformanceMonitoringDecorator(
            ICommand<T> command, 
            IPerformanceLogger logger,
            string commandName) : base(command)
        {
            _logger = logger;
            _commandName = commandName;
        }

        public override async Task<T> ExecuteAsync()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            var result = await _command.ExecuteAsync();
            
            stopwatch.Stop();
            await _logger.LogPerformanceAsync(_commandName, stopwatch.ElapsedMilliseconds);
            
            return result;
        }
    }
}