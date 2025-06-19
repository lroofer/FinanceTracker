using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Repositories
{
    public interface IOperationRepository
    {
        Task<Operation> GetByIdAsync(string id);
        Task<IEnumerable<Operation>> GetAllAsync();
        Task<IEnumerable<Operation>> GetByAccountIdAsync(string accountId);
        Task<IEnumerable<Operation>> GetByCategoryIdAsync(string categoryId);
        Task<IEnumerable<Operation>> GetByDateRangeAsync(DateTime start, DateTime end);
        Task AddAsync(Operation operation);
        Task UpdateAsync(Operation operation);
        Task DeleteAsync(string id);
    }
}
