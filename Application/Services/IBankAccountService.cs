using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Services
{
    public interface IBankAccountService
    {
        Task<BankAccount> GetAccountByIdAsync(string id);
        Task<IEnumerable<BankAccount>> GetAllAccountsAsync();
        Task<BankAccount> CreateAccountAsync(string name, decimal initialBalance = 0);
        Task UpdateAccountNameAsync(string id, string newName);
        Task RecalculateBalanceAsync(string id);
        Task DeleteAccountAsync(string id);
    }
}