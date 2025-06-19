using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Factories;
using FinanceTracker.Domain.Repositories;

namespace FinanceTracker.Application.Services
{
    public class OperationService : IOperationService
    {
        private readonly IOperationRepository _operationRepository;
        private readonly IBankAccountRepository _accountRepository;
        private readonly OperationFactory _operationFactory;

        public OperationService(
            IOperationRepository operationRepository,
            IBankAccountRepository accountRepository,
            OperationFactory operationFactory)
        {
            _operationRepository = operationRepository;
            _accountRepository = accountRepository;
            _operationFactory = operationFactory;
        }

        public async Task<Operation> GetOperationByIdAsync(string id)
        {
            return await _operationRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Operation>> GetAllOperationsAsync()
        {
            return await _operationRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Operation>> GetOperationsByAccountAsync(string accountId)
        {
            return await _operationRepository.GetByAccountIdAsync(accountId);
        }

        public async Task<IEnumerable<Operation>> GetOperationsByCategoryAsync(string categoryId)
        {
            return await _operationRepository.GetByCategoryIdAsync(categoryId);
        }

        public async Task<IEnumerable<Operation>> GetOperationsByDateRangeAsync(DateTime start, DateTime end)
        {
            return await _operationRepository.GetByDateRangeAsync(start, end);
        }

        public async Task<Operation> CreateOperationAsync(OperationType type, string accountId, 
                                                      decimal amount, DateTime date, 
                                                      string categoryId, string description = "")
        {
            var operation = _operationFactory.Create(type, accountId, amount, date, categoryId, description);
            await _operationRepository.AddAsync(operation);
            
            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account == null)
                throw new InvalidOperationException($"Account with ID {accountId} not found");
                
            decimal newBalance = account.Balance;
            
            if (type == OperationType.Income)
                newBalance += amount;
            else
                newBalance -= amount;
                
            account.UpdateBalance(newBalance);
            await _accountRepository.UpdateAsync(account);
            
            return operation;
        }

        public async Task UpdateOperationAmountAsync(string id, decimal newAmount)
        {
            var operation = await _operationRepository.GetByIdAsync(id);
            if (operation == null)
                throw new InvalidOperationException($"Operation with ID {id} not found");
                
            var oldAmount = operation.Amount;
            
            operation.UpdateAmount(newAmount);
            await _operationRepository.UpdateAsync(operation);
            
            var account = await _accountRepository.GetByIdAsync(operation.BankAccountId);
            if (account == null)
                throw new InvalidOperationException($"Account with ID {operation.BankAccountId} not found");
                
            decimal balanceChange;
            
            if (operation.Type == OperationType.Income)
                balanceChange = newAmount - oldAmount;
            else
                balanceChange = oldAmount - newAmount;
                
            account.UpdateBalance(account.Balance + balanceChange);
            await _accountRepository.UpdateAsync(account);
        }

        public async Task UpdateOperationDescriptionAsync(string id, string newDescription)
        {
            var operation = await _operationRepository.GetByIdAsync(id);
            if (operation == null)
                throw new InvalidOperationException($"Operation with ID {id} not found");
                
            operation.UpdateDescription(newDescription);
            await _operationRepository.UpdateAsync(operation);
        }

        public async Task DeleteOperationAsync(string id)
        {
            var operation = await _operationRepository.GetByIdAsync(id);
            if (operation == null)
                throw new InvalidOperationException($"Operation with ID {id} not found");
            
            var account = await _accountRepository.GetByIdAsync(operation.BankAccountId);
            if (account == null)
                throw new InvalidOperationException($"Account with ID {operation.BankAccountId} not found");
                
            decimal balanceChange;
            
            if (operation.Type == OperationType.Income)
                balanceChange = -operation.Amount;
            else
                balanceChange = operation.Amount;
                
            account.UpdateBalance(account.Balance + balanceChange);
            await _accountRepository.UpdateAsync(account);
            
            await _operationRepository.DeleteAsync(id);
        }
    }
}