using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceTracker.Application.Services;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Facades
{
    public class FinanceManagerFacade
    {
        private readonly IBankAccountService _accountService;
        private readonly ICategoryService _categoryService;
        private readonly IOperationService _operationService;
        private readonly IAnalyticsService _analyticsService;

        public FinanceManagerFacade(
            IBankAccountService accountService,
            ICategoryService categoryService,
            IOperationService operationService,
            IAnalyticsService analyticsService)
        {
            _accountService = accountService;
            _categoryService = categoryService;
            _operationService = operationService;
            _analyticsService = analyticsService;
        }

        public async Task<IEnumerable<BankAccount>> GetAllAccountsAsync()
        {
            return await _accountService.GetAllAccountsAsync();
        }

        public async Task<BankAccount> CreateAccountAsync(string name, decimal initialBalance = 0)
        {
            return await _accountService.CreateAccountAsync(name, initialBalance);
        }

        public async Task RecalculateAccountBalanceAsync(string accountId)
        {
            await _accountService.RecalculateBalanceAsync(accountId);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryService.GetAllCategoriesAsync();
        }

        public async Task<Category> CreateCategoryAsync(CategoryType type, string name)
        {
            return await _categoryService.CreateCategoryAsync(type, name);
        }

        public async Task<IEnumerable<Operation>> GetAllOperationsAsync()
        {
            return await _operationService.GetAllOperationsAsync();
        }

        public async Task<Operation> AddIncomeAsync(string accountId, decimal amount, DateTime date, 
                                                 string categoryId, string description = "")
        {
            return await _operationService.CreateOperationAsync(
                OperationType.Income, accountId, amount, date, categoryId, description);
        }

        public async Task<Operation> AddExpenseAsync(string accountId, decimal amount, DateTime date, 
                                                  string categoryId, string description = "")
        {
            return await _operationService.CreateOperationAsync(
                OperationType.Expense, accountId, amount, date, categoryId, description);
        }

        public async Task<decimal> GetBalanceForPeriodAsync(DateTime start, DateTime end)
        {
            return await _analyticsService.CalculateBalanceForPeriodAsync(start, end);
        }

        public async Task<Dictionary<string, decimal>> GetExpensesByCategoryAsync(DateTime start, DateTime end)
        {
            return await _analyticsService.GetExpensesByCategoryAsync(start, end);
        }

        public async Task<Dictionary<string, decimal>> GetIncomeByCategoryAsync(DateTime start, DateTime end)
        {
            return await _analyticsService.GetIncomeByCategoryAsync(start, end);
        }
    }
}