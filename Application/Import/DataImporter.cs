using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceTracker.Application.Services;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Import
{
    public abstract class DataImporter
    {
        protected readonly IBankAccountService _accountService;
        protected readonly ICategoryService _categoryService;
        protected readonly IOperationService _operationService;

        protected DataImporter(
            IBankAccountService accountService,
            ICategoryService categoryService,
            IOperationService operationService)
        {
            _accountService = accountService;
            _categoryService = categoryService;
            _operationService = operationService;
        }

        // Template Method
        public async Task ImportDataAsync(string filePath)
        {
            var data = await ReadDataFromFileAsync(filePath);
            await ImportAccountsAsync(data.Accounts);
            await ImportCategoriesAsync(data.Categories);
            await ImportOperationsAsync(data.Operations);
        }

        // Abstract method to be implemented by subclasses
        protected abstract Task<ImportData> ReadDataFromFileAsync(string filePath);

        // Common methods for all implementations
        protected virtual async Task ImportAccountsAsync(IEnumerable<BankAccountDto> accounts)
        {
            foreach (var accountDto in accounts)
            {
                await _accountService.CreateAccountAsync(accountDto.Name, accountDto.Balance);
            }
        }

        protected virtual async Task ImportCategoriesAsync(IEnumerable<CategoryDto> categories)
        {
            foreach (var categoryDto in categories)
            {
                await _categoryService.CreateCategoryAsync(categoryDto.Type, categoryDto.Name);
            }
        }

        protected virtual async Task ImportOperationsAsync(IEnumerable<OperationDto> operations)
        {
            foreach (var operationDto in operations)
            {
                await _operationService.CreateOperationAsync(
                    operationDto.Type,
                    operationDto.BankAccountId,
                    operationDto.Amount,
                    operationDto.Date,
                    operationDto.CategoryId,
                    operationDto.Description);
            }
        }
    }

    // DTOs for data import
    public class ImportData
    {
        public IEnumerable<BankAccountDto> Accounts { get; set; } = new List<BankAccountDto>();
        public IEnumerable<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
        public IEnumerable<OperationDto> Operations { get; set; } = new List<OperationDto>();
    }

    public class BankAccountDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }

    public class CategoryDto
    {
        public CategoryType Type { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class OperationDto
    {
        public OperationType Type { get; set; }
        public string BankAccountId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public string CategoryId { get; set; } = string.Empty;
    }

}