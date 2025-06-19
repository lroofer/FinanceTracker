using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Factories;
using FinanceTracker.Domain.Repositories;

namespace FinanceTracker.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly CategoryFactory _categoryFactory;

        public CategoryService(
            ICategoryRepository categoryRepository,
            CategoryFactory categoryFactory)
        {
            _categoryRepository = categoryRepository;
            _categoryFactory = categoryFactory;
        }

        public async Task<Category> GetCategoryByIdAsync(string id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesByTypeAsync(CategoryType type)
        {
            return await _categoryRepository.GetByTypeAsync(type);
        }

        public async Task<Category> CreateCategoryAsync(CategoryType type, string name)
        {
            var category = _categoryFactory.Create(type, name);
            await _categoryRepository.AddAsync(category);
            return category;
        }

        public async Task UpdateCategoryNameAsync(string id, string newName)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new InvalidOperationException($"Category with ID {id} not found");
                
            category.Rename(newName);
            await _categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteCategoryAsync(string id)
        {
            await _categoryRepository.DeleteAsync(id);
        }
    }
}