using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(string id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<IEnumerable<Category>> GetByTypeAsync(CategoryType type);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(string id);
    }
}