using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;

namespace FinanceTracker.Infrastructure.Repositories
{
    public class InMemoryOperationRepository : IOperationRepository
    {
        private readonly ConcurrentDictionary<string, Operation> _operations = new();

        public Task<Operation> GetByIdAsync(string id)
        {
            _operations.TryGetValue(id, out var operation);
            return Task.FromResult(operation);
        }

        public Task<IEnumerable<Operation>> GetAllAsync()
        {
            return Task.FromResult(_operations.Values.AsEnumerable());
        }

        public Task<IEnumerable<Operation>> GetByAccountIdAsync(string accountId)
        {
            var operations = _operations.Values.Where(o => o.BankAccountId == accountId);
            return Task.FromResult(operations);
        }

        public Task<IEnumerable<Operation>> GetByCategoryIdAsync(string categoryId)
        {
            var operations = _operations.Values.Where(o => o.CategoryId == categoryId);
            return Task.FromResult(operations);
        }

        public Task<IEnumerable<Operation>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            var operations = _operations.Values.Where(o => o.Date >= start && o.Date <= end);
            return Task.FromResult(operations);
        }

        public Task AddAsync(Operation operation)
        {
            _operations.TryAdd(operation.Id, operation);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Operation operation)
        {
            _operations.TryUpdate(operation.Id, operation, _operations[operation.Id]);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(string id)
        {
            _operations.TryRemove(id, out _);
            return Task.CompletedTask;
        }
    }
}