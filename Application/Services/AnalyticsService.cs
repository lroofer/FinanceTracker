using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;

namespace FinanceTracker.Application.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IOperationRepository _operationRepository;
        private readonly ICategoryRepository _categoryRepository;

        public AnalyticsService(
            IOperationRepository operationRepository,
            ICategoryRepository categoryRepository)
        {
            _operationRepository = operationRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<decimal> CalculateBalanceForPeriodAsync(DateTime start, DateTime end)
        {
            var operations = await _operationRepository.GetByDateRangeAsync(start, end);
            
            decimal income = operations
                .Where(o => o.Type == OperationType.Income)
                .Sum(o => o.Amount);
                
            decimal expenses = operations
                .Where(o => o.Type == OperationType.Expense)
                .Sum(o => o.Amount);
                
            return income - expenses;
        }

        public async Task<Dictionary<string, decimal>> GetExpensesByCategoryAsync(DateTime start, DateTime end)
        {
            var operations = await _operationRepository.GetByDateRangeAsync(start, end);
            var expenses = operations.Where(o => o.Type == OperationType.Expense);
            
            var categories = await _categoryRepository.GetAllAsync();
            var categoryDict = categories.ToDictionary(c => c.Id, c => c.Name);
            
            return expenses
                .GroupBy(o => o.CategoryId)
                .ToDictionary(
                    g => categoryDict.ContainsKey(g.Key) ? categoryDict[g.Key] : "Unknown",
                    g => g.Sum(o => o.Amount)
                );
        }

        public async Task<Dictionary<string, decimal>> GetIncomeByCategoryAsync(DateTime start, DateTime end)
        {
            var operations = await _operationRepository.GetByDateRangeAsync(start, end);
            var incomes = operations.Where(o => o.Type == OperationType.Income);
            
            var categories = await _categoryRepository.GetAllAsync();
            var categoryDict = categories.ToDictionary(c => c.Id, c => c.Name);
            
            return incomes
                .GroupBy(o => o.CategoryId)
                .ToDictionary(
                    g => categoryDict.ContainsKey(g.Key) ? categoryDict[g.Key] : "Unknown",
                    g => g.Sum(o => o.Amount)
                );
        }

        public async Task<Dictionary<DateTime, decimal>> GetDailyBalanceAsync(DateTime start, DateTime end)
        {
            var operations = await _operationRepository.GetByDateRangeAsync(start, end);
            
            return operations
                .GroupBy(o => o.Date.Date)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(o => o.Type == OperationType.Income ? o.Amount : -o.Amount)
                );
        }
    }
}