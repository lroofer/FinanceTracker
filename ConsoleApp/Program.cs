using System;
using System.Threading.Tasks;
using FinanceTracker.Application.Commands;
using FinanceTracker.Application.Facades;
using FinanceTracker.Application.Services;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Factories;
using FinanceTracker.Application.Logging;
using FinanceTracker.Infrastructure.Repositories;

namespace FinanceTracker.ConsoleApp
{
    class Program
    {
        private static FinanceManagerFacade _financeManager;
        private static IPerformanceLogger _performanceLogger;

        static async Task Main(string[] args)
        {
            SetupDependencies();
            await RunApplication();
        }

        private static void SetupDependencies()
        {
            // Setup repositories
            var accountRepository = new InMemoryBankAccountRepository();
            var categoryRepository = new InMemoryCategoryRepository();
            var operationRepository = new InMemoryOperationRepository();

            // Setup factories
            var accountFactory = new BankAccountFactory();
            var categoryFactory = new CategoryFactory();
            var operationFactory = new OperationFactory();

            // Setup services
            var accountService = new BankAccountService(accountRepository, operationRepository, accountFactory);
            var categoryService = new CategoryService(categoryRepository, categoryFactory);
            var operationService = new OperationService(operationRepository, accountRepository, operationFactory);
            var analyticsService = new AnalyticsService(operationRepository, categoryRepository);

            // Setup facade
            _financeManager = new FinanceManagerFacade(accountService, categoryService, operationService, analyticsService);

            // Setup logging
            _performanceLogger = new ConsolePerformanceLogger();
        }

        private static async Task RunApplication()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== Finance Tracker ===");
                Console.WriteLine("1. Manage Accounts");
                Console.WriteLine("2. Manage Categories");
                Console.WriteLine("3. Manage Operations");
                Console.WriteLine("4. View Analytics");
                Console.WriteLine("5. Exit");
                Console.Write("\nSelect option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await ManageAccounts();
                        break;
                    case "2":
                        await ManageCategories();
                        break;
                    case "3":
                        await ManageOperations();
                        break;
                    case "4":
                        await ViewAnalytics();
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static async Task ManageAccounts()
        {
            Console.Clear();
            Console.WriteLine("=== Manage Accounts ===");
            Console.WriteLine("1. View all accounts");
            Console.WriteLine("2. Create new account");
            Console.WriteLine("3. Recalculate account balance");
            Console.WriteLine("4. Back");
            Console.Write("\nSelect option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ViewAllAccounts();
                    break;
                case "2":
                    await CreateNewAccount();
                    break;
                case "3":
                    await RecalculateBalance();
                    break;
            }
        }

        private static async Task ViewAllAccounts()
        {
            Console.Clear();
            Console.WriteLine("=== All Accounts ===");

            var accounts = await _financeManager.GetAllAccountsAsync();

            foreach (var account in accounts)
            {
                Console.WriteLine($"ID: {account.Id}");
                Console.WriteLine($"Name: {account.Name}");
                Console.WriteLine($"Balance: ${account.Balance:F2}");
                Console.WriteLine("---");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static async Task CreateNewAccount()
        {
            Console.Clear();
            Console.WriteLine("=== Create New Account ===");

            Console.Write("Enter account name: ");
            var name = Console.ReadLine();

            Console.Write("Enter initial balance (0 for empty): ");
            if (!decimal.TryParse(Console.ReadLine(), out var balance))
                balance = 0;

            try
            {
                var account = await _financeManager.CreateAccountAsync(name, balance);
                
                Console.WriteLine($"\nAccount created successfully!");
                Console.WriteLine($"ID: {account.Id}");
                Console.WriteLine($"Name: {account.Name}");
                Console.WriteLine($"Balance: ${account.Balance:F2}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }


        private static async Task RecalculateBalance()
        {
            Console.Clear();
            Console.WriteLine("=== Recalculate Account Balance ===");

            Console.Write("Enter account ID: ");
            var accountId = Console.ReadLine();

            try
            {
                await _financeManager.RecalculateAccountBalanceAsync(accountId);
                Console.WriteLine("Balance recalculated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static async Task ManageCategories()
        {
            Console.Clear();
            Console.WriteLine("=== Manage Categories ===");
            Console.WriteLine("1. View all categories");
            Console.WriteLine("2. Create new category");
            Console.WriteLine("3. Back");
            Console.Write("\nSelect option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ViewAllCategories();
                    break;
                case "2":
                    await CreateNewCategory();
                    break;
            }
        }

        private static async Task ViewAllCategories()
        {
            Console.Clear();
            Console.WriteLine("=== All Categories ===");

            var categories = await _financeManager.GetAllCategoriesAsync();

            Console.WriteLine("\nIncome Categories:");
            foreach (var category in categories)
            {
                if (category.Type == CategoryType.Income)
                {
                    Console.WriteLine($"- {category.Name} (ID: {category.Id})");
                }
            }

            Console.WriteLine("\nExpense Categories:");
            foreach (var category in categories)
            {
                if (category.Type == CategoryType.Expense)
                {
                    Console.WriteLine($"- {category.Name} (ID: {category.Id})");
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static async Task CreateNewCategory()
        {
            Console.Clear();
            Console.WriteLine("=== Create New Category ===");

            Console.WriteLine("Select category type:");
            Console.WriteLine("1. Income");
            Console.WriteLine("2. Expense");
            Console.Write("Choice: ");

            var typeChoice = Console.ReadLine();
            CategoryType type = typeChoice == "1" ? CategoryType.Income : CategoryType.Expense;

            Console.Write("Enter category name: ");
            var name = Console.ReadLine();

            var category = await _financeManager.CreateCategoryAsync(type, name);

            Console.WriteLine($"\nCategory created successfully!");
            Console.WriteLine($"ID: {category.Id}");
            Console.WriteLine($"Name: {category.Name}");
            Console.WriteLine($"Type: {category.Type}");

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static async Task ManageOperations()
        {
            Console.Clear();
            Console.WriteLine("=== Manage Operations ===");
            Console.WriteLine("1. View all operations");
            Console.WriteLine("2. Add income");
            Console.WriteLine("3. Add expense");
            Console.WriteLine("4. Back");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ViewAllOperations();
                    break;
                case "2":
                    await AddIncome();
                    break;
                case "3":
                    await AddExpense();
                    break;
            }
        }

        private static async Task ViewAllOperations()
        {
            Console.Clear();
            Console.WriteLine("=== All Operations ===");

            var operations = await _financeManager.GetAllOperationsAsync();

            foreach (var operation in operations)
            {
                var sign = operation.Type == OperationType.Income ? "+" : "-";
                Console.WriteLine($"{operation.Date:yyyy-MM-dd} | {sign}${operation.Amount:F2} | {operation.Description}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static async Task AddIncome()
        {
            await AddOperation(OperationType.Income);
        }

        private static async Task AddExpense()
        {
            await AddOperation(OperationType.Expense);
        }

        private static async Task AddOperation(OperationType type)
        {
            Console.Clear();
            Console.WriteLine($"=== Add {type} ===");

            Console.Write("Enter account ID: ");
            var accountId = Console.ReadLine();

            Console.Write("Enter amount: ");
            if (!decimal.TryParse(Console.ReadLine(), out var amount))
            {
                Console.WriteLine("Invalid amount!");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter category ID: ");
            var categoryId = Console.ReadLine();

            Console.Write("Enter description (optional): ");
            var description = Console.ReadLine();

            Console.Write("Enter date (YYYY-MM-DD) or press Enter for today: ");
            var dateInput = Console.ReadLine();
            var date = string.IsNullOrEmpty(dateInput) ? DateTime.Today : DateTime.Parse(dateInput);

            try
            {
                Operation operation;
                if (type == OperationType.Income)
                {
                    operation = await _financeManager.AddIncomeAsync(accountId, amount, date, categoryId, description);
                }
                else
                {
                    operation = await _financeManager.AddExpenseAsync(accountId, amount, date, categoryId, description);
                }

                Console.WriteLine($"\n{type} added successfully!");
                Console.WriteLine($"Amount: ${operation.Amount:F2}");
                Console.WriteLine($"Date: {operation.Date:yyyy-MM-dd}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static async Task ViewAnalytics()
        {
            Console.Clear();
            Console.WriteLine("=== Analytics ===");
            Console.WriteLine("1. View balance for period");
            Console.WriteLine("2. View expenses by category");
            Console.WriteLine("3. View income by category");
            Console.WriteLine("4. Back");
            Console.Write("\nSelect option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ViewBalanceForPeriod();
                    break;
                case "2":
                    await ViewExpensesByCategory();
                    break;
                case "3":
                    await ViewIncomeByCategory();
                    break;
            }
        }

        private static async Task ViewBalanceForPeriod()
        {
            Console.Clear();
            Console.WriteLine("=== Balance for Period ===");

            Console.Write("Enter start date (YYYY-MM-DD): ");
            var startDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Enter end date (YYYY-MM-DD): ");
            var endDate = DateTime.Parse(Console.ReadLine());

            var balance = await _financeManager.GetBalanceForPeriodAsync(startDate, endDate);

            Console.WriteLine($"\nBalance for period {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}: ${balance:F2}");

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static async Task ViewExpensesByCategory()
        {
            Console.Clear();
            Console.WriteLine("=== Expenses by Category ===");

            Console.Write("Enter start date (YYYY-MM-DD): ");
            var startDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Enter end date (YYYY-MM-DD): ");
            var endDate = DateTime.Parse(Console.ReadLine());

            var expenses = await _financeManager.GetExpensesByCategoryAsync(startDate, endDate);

            Console.WriteLine($"\nExpenses for period {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}:");
            foreach (var kvp in expenses)
            {
                Console.WriteLine($"{kvp.Key}: ${kvp.Value:F2}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static async Task ViewIncomeByCategory()
        {
            Console.Clear();
            Console.WriteLine("=== Income by Category ===");

            Console.Write("Enter start date (YYYY-MM-DD): ");
            var startDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Enter end date (YYYY-MM-DD): ");
            var endDate = DateTime.Parse(Console.ReadLine());

            var income = await _financeManager.GetIncomeByCategoryAsync(startDate, endDate);

            Console.WriteLine($"\nIncome for period {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}:");
            foreach (var kvp in income)
            {
                Console.WriteLine($"{kvp.Key}: ${kvp.Value:F2}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}