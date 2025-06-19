using System;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Factories
{
    public class OperationFactory
    {
        public Operation Create(OperationType type, string bankAccountId, decimal amount, 
                              DateTime date, string categoryId, string description = "")
        {
            if (string.IsNullOrWhiteSpace(bankAccountId))
                throw new ArgumentException("Bank account ID cannot be empty", nameof(bankAccountId));
                
            if (string.IsNullOrWhiteSpace(categoryId))
                throw new ArgumentException("Category ID cannot be empty", nameof(categoryId));
                
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));
                
            return Operation.Create(type, bankAccountId, amount, date, categoryId, description);
        }
    }
}