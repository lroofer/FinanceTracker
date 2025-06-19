using System;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Factories
{
    public class CategoryFactory
    {
        public Category Create(CategoryType type, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name cannot be empty", nameof(name));
                
            return Category.Create(type, name);
        }
    }
}
