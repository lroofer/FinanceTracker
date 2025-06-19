using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;

namespace FinanceTracker.Infrastructure.Repositories
{
    public class InMemoryBankAccountRepository : IBankAccountRepository
    {
        private readonly ConcurrentDictionary<string, BankAccount> _accounts = new();

        public Task<BankAccount?> GetByIdAsync(string id)
        {
            _accounts.TryGetValue(id, out var account);
            return Task.FromResult(account);
        }


        public Task<IEnumerable<BankAccount>> GetAllAsync()
        {
            return Task.FromResult(_accounts.Values.AsEnumerable());
        }

        public Task AddAsync(BankAccount account)
        {
            _accounts.TryAdd(account.Id, account);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(BankAccount account)
        {
            _accounts.TryUpdate(account.Id, account, _accounts[account.Id]);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(string id)
        {
            _accounts.TryRemove(id, out _);
            return Task.CompletedTask;
        }
    }
}