using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Services
{
    public interface ICategoryService
    {
        Task<Category> GetCategoryByIdAsync(string id);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<Category>> GetCategoriesByTypeAsync(CategoryType type);
        Task<Category> CreateCategoryAsync(CategoryType type, string name);
        Task UpdateCategoryNameAsync(string id, string newName);
        Task DeleteCategoryAsync(string id);
    }
}