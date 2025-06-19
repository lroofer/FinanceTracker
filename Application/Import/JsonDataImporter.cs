using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FinanceTracker.Application.Services;

namespace FinanceTracker.Application.Import
{
    public class JsonDataImporter : DataImporter
    {
        public JsonDataImporter(
            IBankAccountService accountService,
            ICategoryService categoryService,
            IOperationService operationService) 
            : base(accountService, categoryService, operationService)
        {
        }

        protected override async Task<ImportData> ReadDataFromFileAsync(string filePath)
        {
            var json = await File.ReadAllTextAsync(filePath);
            return JsonConvert.DeserializeObject<ImportData>(json);
        }
    }
}