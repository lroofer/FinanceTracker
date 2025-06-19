using System;
using System.Threading.Tasks;
using FinanceTracker.Application.Services;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Commands
{
    public class AddIncomeCommand : ICommand<Operation>
    {
        private readonly IOperationService _operationService;
        private readonly string _accountId;
        private readonly decimal _amount;
        private readonly DateTime _date;
        private readonly string _categoryId;
        private readonly string _description;

        public AddIncomeCommand(
            IOperationService operationService,
            string accountId,
            decimal amount,
            DateTime date,
            string categoryId,
            string description = "")
        {
            _operationService = operationService;
            _accountId = accountId;
            _amount = amount;
            _date = date;
            _categoryId = categoryId;
            _description = description;
        }

        public async Task<Operation> ExecuteAsync()
        {
            return await _operationService.CreateOperationAsync(
                OperationType.Income, _accountId, _amount, _date, _categoryId, _description);
        }
    }
}