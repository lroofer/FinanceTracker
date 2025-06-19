using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Repositories;

namespace FinanceTracker.Infrastructure.Repositories
{
    public class InMemoryCategoryRepository : ICategoryRepository
    {
        private readonly ConcurrentDictionary<string, Category> _categories = new();

        public Task<Category> GetByIdAsync(string id)
        {
            _categories.TryGetValue(id, out var category);
            return Task.FromResult(category);
        }

        public Task<IEnumerable<Category>> GetAllAsync()
        {
            return Task.FromResult(_categories.Values.AsEnumerable());
        }

        public Task<IEnumerable<Category>> GetByTypeAsync(CategoryType type)
        {
            var categories = _categories.Values.Where(c => c.Type == type);
            return Task.FromResult(categories);
        }

        public Task AddAsync(Category category)
        {
            _categories.TryAdd(category.Id, category);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Category category)
        {
            _categories.TryUpdate(category.Id, category, _categories[category.Id]);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(string id)
        {
            _categories.TryRemove(id, out _);
            return Task.CompletedTask;
        }
    }
}