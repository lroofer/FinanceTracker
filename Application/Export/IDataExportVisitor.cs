using System.Threading.Tasks;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Export
{
    public interface IDataExportVisitor
    {
        Task VisitAccount(BankAccount account);
        Task VisitCategory(Category category);
        Task VisitOperation(Operation operation);
        Task<string> GetExportedData();
    }
}
