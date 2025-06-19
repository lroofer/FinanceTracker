using System.Threading.Tasks;
using FinanceTracker.Application.Services;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Commands
{
    public class CreateAccountCommand : ICommand<BankAccount>
    {
        private readonly IBankAccountService _accountService;
        private readonly string _name;
        private readonly decimal _initialBalance;

        public CreateAccountCommand(
            IBankAccountService accountService,
            string name,
            decimal initialBalance = 0)
        {
            _accountService = accountService;
            _name = name;
            _initialBalance = initialBalance;
        }

        public async Task<BankAccount> ExecuteAsync()
        {
            return await _accountService.CreateAccountAsync(_name, _initialBalance);
        }
    }
}