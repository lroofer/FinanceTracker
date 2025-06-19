using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Services
{
    public interface IOperationService
    {
        Task<Operation> GetOperationByIdAsync(string id);
        Task<IEnumerable<Operation>> GetAllOperationsAsync();
        Task<IEnumerable<Operation>> GetOperationsByAccountAsync(string accountId);
        Task<IEnumerable<Operation>> GetOperationsByCategoryAsync(string categoryId);
        Task<IEnumerable<Operation>> GetOperationsByDateRangeAsync(DateTime start, DateTime end);
        Task<Operation> CreateOperationAsync(OperationType type, string accountId, decimal amount, 
                                          DateTime date, string categoryId, string description = "");
        Task UpdateOperationAmountAsync(string id, decimal newAmount);
        Task UpdateOperationDescriptionAsync(string id, string newDescription);
        Task DeleteOperationAsync(string id);
    }
}