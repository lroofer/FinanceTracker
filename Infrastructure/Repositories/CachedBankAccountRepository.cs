using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;

namespace FinanceTracker.Infrastructure.Repositories
{
    public class CachedBankAccountRepository : IBankAccountRepository
    {
        private readonly IBankAccountRepository _innerRepository;
        private readonly Dictionary<string, BankAccount> _cache = new();

        public CachedBankAccountRepository(IBankAccountRepository innerRepository)
        {
            _innerRepository = innerRepository;
        }

        public async Task<BankAccount> GetByIdAsync(string id)
        {
            if (_cache.ContainsKey(id))
                return _cache[id];

            var account = await _innerRepository.GetByIdAsync(id);
            if (account != null)
                _cache[id] = account;

            return account;
        }

        public async Task<IEnumerable<BankAccount>> GetAllAsync()
        {
            return await _innerRepository.GetAllAsync();
        }

        public async Task AddAsync(BankAccount account)
        {
            await _innerRepository.AddAsync(account);
            _cache[account.Id] = account;
        }

        public async Task UpdateAsync(BankAccount account)
        {
            await _innerRepository.UpdateAsync(account);
            _cache[account.Id] = account;
        }

        public async Task DeleteAsync(string id)
        {
            await _innerRepository.DeleteAsync(id);
            _cache.Remove(id);
        }
    }
}