using System;

namespace FinanceTracker.Domain.Entities
{
    public enum OperationType
    {
        Income,
        Expense
    }

    public class Operation
    {
        public string Id { get; private set; }
        public OperationType Type { get; private set; }
        public string BankAccountId { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime Date { get; private set; }
        public string Description { get; private set; }
        public string CategoryId { get; private set; }

        private Operation(string id, OperationType type, string bankAccountId, 
                        decimal amount, DateTime date, string description, string categoryId)
        {
            Id = id;
            Type = type;
            BankAccountId = bankAccountId;
            Amount = amount;
            Date = date;
            Description = description;
            CategoryId = categoryId;
        }

        public static Operation Create(OperationType type, string bankAccountId, 
                                    decimal amount, DateTime date, string categoryId, 
                                    string description = "")
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));

            return new Operation(
                Guid.NewGuid().ToString(),
                type,
                bankAccountId,
                amount,
                date,
                description,
                categoryId
            );
        }

        public void UpdateAmount(decimal newAmount)
        {
            if (newAmount <= 0)
                throw new ArgumentException("Amount must be positive", nameof(newAmount));
                
            Amount = newAmount;
        }

        public void UpdateDescription(string newDescription)
        {
            Description = newDescription ?? string.Empty;
        }
    }
}