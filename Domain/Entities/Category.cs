using System;

namespace FinanceTracker.Domain.Entities
{
    public enum CategoryType
    {
        Income,
        Expense
    }

    public class Category
    {
        public string Id { get; private set; }
        public CategoryType Type { get; private set; }
        public string Name { get; private set; }

        private Category(string id, CategoryType type, string name)
        {
            Id = id;
            Type = type;
            Name = name;
        }

        public static Category Create(CategoryType type, string name)
        {
            return new Category(Guid.NewGuid().ToString(), type, name);
        }

        public void Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Category name cannot be empty");
                
            Name = newName;
        }
    }
}