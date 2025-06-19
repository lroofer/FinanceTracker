using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceTracker.Application.Services
{
    public interface IAnalyticsService
    {
        Task<decimal> CalculateBalanceForPeriodAsync(DateTime start, DateTime end);
        Task<Dictionary<string, decimal>> GetExpensesByCategoryAsync(DateTime start, DateTime end);
        Task<Dictionary<string, decimal>> GetIncomeByCategoryAsync(DateTime start, DateTime end);
        Task<Dictionary<DateTime, decimal>> GetDailyBalanceAsync(DateTime start, DateTime end);
    }
}