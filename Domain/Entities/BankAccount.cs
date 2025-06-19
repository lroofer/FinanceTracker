using System;

namespace FinanceTracker.Domain.Entities
{
    public class BankAccount
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public decimal Balance { get; private set; }

        private BankAccount(string id, string name, decimal balance)
        {
            Id = id;
            Name = name;
            Balance = balance;
        }

        public static BankAccount Create(string name, decimal initialBalance = 0)
        {
            return new BankAccount(Guid.NewGuid().ToString(), name, initialBalance);
        }

        public void UpdateBalance(decimal newBalance)
        {
            Balance = newBalance;
        }

        public void Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Account name cannot be empty");
                
            Name = newName;
        }
    }
}