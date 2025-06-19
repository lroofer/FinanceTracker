using System;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Factories
{
    public class BankAccountFactory
    {
        public BankAccount Create(string name, decimal initialBalance = 0)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Bank account name cannot be empty", nameof(name));
                
            if (initialBalance < 0)
                throw new ArgumentException("Initial balance cannot be negative", nameof(initialBalance));
                
            return BankAccount.Create(name, initialBalance);
        }
    }
}