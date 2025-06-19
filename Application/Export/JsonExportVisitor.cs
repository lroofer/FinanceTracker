using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Export
{
    public class JsonExportVisitor : IDataExportVisitor
    {
        private readonly List<object> _exportData = new List<object>();

        public Task VisitAccount(BankAccount account)
        {
            _exportData.Add(new
            {
                Type = "Account",
                account.Id,
                account.Name,
                account.Balance
            });
            return Task.CompletedTask;
        }

        public Task VisitCategory(Category category)
        {
            _exportData.Add(new
            {
                Type = "Category",
                category.Id,
                CategoryType = category.Type.ToString(),
                category.Name
            });
            return Task.CompletedTask;
        }

        public Task VisitOperation(Operation operation)
        {
            _exportData.Add(new
            {
                Type = "Operation",
                operation.Id,
                OperationType = operation.Type.ToString(),
                operation.BankAccountId,
                operation.Amount,
                operation.Date,
                operation.Description,
                operation.CategoryId
            });
            return Task.CompletedTask;
        }

        public Task<string> GetExportedData()
        {
            return Task.FromResult(JsonConvert.SerializeObject(_exportData, Formatting.Indented));
        }
    }
}