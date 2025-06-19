using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Factories;
using FinanceTracker.Domain.Repositories;

namespace FinanceTracker.Application.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository _accountRepository;
        private readonly IOperationRepository _operationRepository;
        private readonly BankAccountFactory _accountFactory;

        public BankAccountService(
            IBankAccountRepository accountRepository,
            IOperationRepository operationRepository,
            BankAccountFactory accountFactory)
        {
            _accountRepository = accountRepository;
            _operationRepository = operationRepository;
            _accountFactory = accountFactory;
        }

        public async Task<IEnumerable<BankAccount>> GetAllAccountsAsync()
        {
            return await _accountRepository.GetAllAsync();
        }

        public async Task<BankAccount> CreateAccountAsync(string name, decimal initialBalance = 0)
        {
            var account = _accountFactory.Create(name, initialBalance);
            await _accountRepository.AddAsync(account);
            return account;
        }

        public async Task<BankAccount?> GetAccountByIdAsync(string id)
        {
            return await _accountRepository.GetByIdAsync(id);
        }

        public async Task UpdateAccountNameAsync(string id, string newName)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
                throw new InvalidOperationException($"Account with ID {id} not found");
                
            account.Rename(newName);
            await _accountRepository.UpdateAsync(account);
        }


        public async Task RecalculateBalanceAsync(string id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
                throw new InvalidOperationException($"Account with ID {id} not found");
                
            var operations = await _operationRepository.GetByAccountIdAsync(id);
            
            decimal balance = 0;
            
            foreach (var op in operations)
            {
                if (op.Type == OperationType.Income)
                    balance += op.Amount;
                else
                    balance -= op.Amount;
            }
            
            account.UpdateBalance(balance);
            await _accountRepository.UpdateAsync(account);
        }

        public async Task DeleteAccountAsync(string id)
        {
            await _accountRepository.DeleteAsync(id);
        }
    }
}
