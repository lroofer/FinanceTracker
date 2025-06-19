using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Repositories
{
    public interface IBankAccountRepository
    {
        Task<BankAccount?> GetByIdAsync(string id);
        Task<IEnumerable<BankAccount>> GetAllAsync();
        Task AddAsync(BankAccount account);
        Task UpdateAsync(BankAccount account);
        Task DeleteAsync(string id);
    }

}